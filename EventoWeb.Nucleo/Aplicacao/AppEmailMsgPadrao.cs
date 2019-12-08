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

        public void EnviarCodigoCriacaoInscricao(InscricaoParticipante inscricao, string codigo)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoEmail.Configuracao = ObterCnfEmail(inscricao.Evento.Id);
            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoCodigoAcessoCriacao.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<EmailCodigoInscricao>(mensagem.MensagemInscricaoCodigoAcessoCriacao.Mensagem, 
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

        public void EnviarCodigoAcompanhamentoInscricao(InscricaoParticipante inscricao, string codigo)
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

        public void EnviarInscricaoRegistrada(InscricaoParticipante inscricao)
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

            var idCrianca = new AppInscOnLineIdentificacaoInfantil();
            dto.Criancas = Contexto.RepositorioInscricoes.ListarInscricoesInfantisDoResponsavel(inscricao)
                .Select(x => 
                {
                    var crianca = x.ConverterComCodigo();
                    crianca.Codigo = idCrianca.GerarCodigo(x.Id);
                    return crianca;
                })
                .ToList();
           /* foreach(var dtoCrianca in dto.Criancas)
            {
                dtoCrianca.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(dtoCrianca.Id.Value)
                        .Select(x => x.ConverterComCodigo()).ToList();
            }*/

            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoRegistrada.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<DTOInscricaoCompletaCodigo>(mensagem.MensagemInscricaoRegistrada.Mensagem, dto),
                Endereco = inscricao.Pessoa.Email
            });
        }

        public void EnviarInscricaoAceita(InscricaoParticipante inscricao)
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

        public void Testar(int idInscricao)
        {
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(idInscricao);
                EnviarInscricaoRegistrada((InscricaoParticipante)inscricao);
            });
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
