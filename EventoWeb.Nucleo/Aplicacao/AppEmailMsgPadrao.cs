using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppEmailMsgPadrao : AppBase
    {
        private readonly AServicoEmail m_ServicoEmail;
        private readonly AGeracaoMensagemEmail m_GeradorMsgEmail;

        public AppEmailMsgPadrao(IContexto contexto, AServicoEmail servicoEmail, AGeracaoMensagemEmail geradorMsgEmail) : base(contexto)
        {
            m_ServicoEmail = servicoEmail;
            m_GeradorMsgEmail = geradorMsgEmail;
        }

        public void EnviarCodigoValidacaoEmail(int idEvento, string email, string codigo)
        {
            var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
            var mensagem = ObterMensagem(idEvento);
            m_ServicoEmail.Configuracao = ObterCnfEmail(idEvento);
            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoCodigoAcessoCriacao.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<EmailValidacaoEmail>(mensagem.MensagemInscricaoCodigoAcessoCriacao.Mensagem, 
                    new EmailValidacaoEmail
                    {
                        Codigo = codigo,
                        Evento = evento.Nome
                    }
                ),
                Endereco = email
            });
        }

        public void EnviarCodigoAcompanhamentoInscricao(Inscricao inscricao, string codigo)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoEmail.Configuracao = ObterCnfEmail(inscricao.Evento.Id);
            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoCodigoAcessoAcompanhamento.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<EmailCodigoInscricao>(mensagem.MensagemInscricaoCodigoAcessoAcompanhamento.Mensagem,
                    new EmailCodigoInscricao
                    {
                        Codigo = codigo,
                        Evento = inscricao.Evento.Nome,
                        NomePessoa = inscricao.Pessoa.Nome,
                        Cidade = inscricao.Pessoa.Endereco.Cidade,
                        Identificacao = new AppInscOnLineIdentificacaoInscricao().GerarCodigo(inscricao.Id),
                        UF = inscricao.Pessoa.Endereco.UF
                    }
                ),
                Endereco = inscricao.Pessoa.Email
            });
        }

        public void EnviarInscricaoRegistradaAdulto(InscricaoParticipante inscricao)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoEmail.Configuracao = ObterCnfEmail(inscricao.Evento.Id);

            var dto = inscricao.ConverterComCodigo();
            dto.Codigo = new AppInscOnLineIdentificacaoInscricao().GerarCodigo(inscricao.Id);

            var idSarau = new AppInscOnLineIdentificacaoSarau();
            dto.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(inscricao.Id)
                        .Select(x => 
                        {
                            var sarau = x.ConverterComCodigo();
                            sarau.Codigo = idSarau.GerarCodigo(x.Id);
                            return sarau;
                        })
                        .ToList();

            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoRegistradaAdulto.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<DTOInscricaoCompletaAdultoCodigo>(mensagem.MensagemInscricaoRegistradaAdulto.Mensagem, dto),
                Endereco = inscricao.Pessoa.Email
            });
        }

        public void EnviarInscricaoRegistradaInfantil(InscricaoInfantil inscricao)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoEmail.Configuracao = ObterCnfEmail(inscricao.Evento.Id);

            var dto = inscricao.ConverterComCodigo();
            dto.Codigo = new AppInscOnLineIdentificacaoInscricao().GerarCodigo(inscricao.Id);

            var idSarau = new AppInscOnLineIdentificacaoSarau();
            dto.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(inscricao.Id)
                        .Select(x =>
                        {
                            var sarau = x.ConverterComCodigo();
                            sarau.Codigo = idSarau.GerarCodigo(x.Id);
                            return sarau;
                        })
                        .ToList();

            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoRegistradaAdulto.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<DTOInscricaoCompletaInfantilCodigo>(mensagem.MensagemInscricaoRegistradaAdulto.Mensagem, dto),
                Endereco = inscricao.Pessoa.Email
            });
        }

        public void EnviarInscricaoAceita(Inscricao inscricao)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoEmail.Configuracao = ObterCnfEmail(inscricao.Evento.Id);
            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoConfirmada.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<EmailConfirmacaoInscricao>(mensagem.MensagemInscricaoConfirmada.Mensagem,
                    new EmailConfirmacaoInscricao
                    {
                        Evento = inscricao.Evento.Nome,
                        NomePessoa = inscricao.Pessoa.Nome,
                    }
                ),
                Endereco = inscricao.Pessoa.Email
            });
        }

        public void EnviarInscricaoRejeitada(InscricaoParticipante inscricao)
        {
            
        }

        private MensagemEmailPadrao ObterMensagem(int idEvento)
        {
            return Contexto.RepositorioMensagensEmailPadrao.Obter(idEvento) ??
                throw new ExcecaoAplicacao("AppEmailMsgPadrao", "Mensagem Padrão de Email não foi cadastrada");
        }

        private ConfiguracaoEmail ObterCnfEmail(int idEvento)
        {
            return Contexto.RepositorioConfiguracoesEmail.Obter(idEvento) ??
                throw new ExcecaoAplicacao("AppEmailMsgPadrao", "Configuração de Email não foi cadastrada");
        }
    }

    public class EmailValidacaoEmail
    {
        public string Evento { get; set; }
        public string Codigo { get; set; }
    }

    public class EmailCodigoInscricao
    {
        public string Evento { get; set; }
        public string Identificacao { get; set; }
        public string NomePessoa { get; set; }
        public string Codigo { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
    }

    public class EmailConfirmacaoInscricao
    {
        public string NomePessoa { get; set; }
        public string Evento { get; set; }
    }
}
