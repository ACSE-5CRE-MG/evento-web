using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
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

        public void EnviarInscricaoRegistrada(int idEvento, InscricaoParticipante inscricao)
        {
            var mensagem = ObterMensagem(idEvento);
            m_ServicoEmail.Configuracao = ObterCnfEmail(idEvento);


            var dto = inscricao.Converter();
            dto.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(inscricao.Id)
                        .Select(x => x.Converter()).ToList();

            dto.Criancas = Contexto.RepositorioInscricoes.ListarInscricoesInfantisDoResponsavel(inscricao)
                .Select(x => x.Converter())
                .ToList();
            foreach(var dtoCrianca in dto.Criancas)
            {
                dtoCrianca.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(dtoCrianca.Id.Value)
                        .Select(x => x.Converter()).ToList();
            }

            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoRegistrada.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<DTOInscricaoCompleta>(mensagem.MensagemInscricaoRegistrada.Mensagem, dto),
                Endereco = inscricao.Pessoa.Email
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
}
