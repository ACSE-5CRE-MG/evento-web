using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Negocio.Servicos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class ServicoEmail : IServicoEmail
    {
        private readonly IContexto m_Contexto;

        public ServicoEmail(IContexto contexto)
        {
            m_Contexto = contexto;
        }

        public void Enviar(Evento evento, ModeloMensagem mensagem, string emailDestino)
        {
            var configuracao = m_Contexto.RepositorioConfiguracoesEmail.Obter(evento.Id);
            if (configuracao != null)
            {
                using (SmtpClient clienteSmtp = new SmtpClient(configuracao.ServidorEmail, configuracao.PortaServidor.Value))
                {
                    clienteSmtp.EnableSsl = configuracao.TipoSeguranca.Value == TipoSegurancaEmail.SSL;
                    clienteSmtp.Credentials = new NetworkCredential(configuracao.UsuarioEmail, configuracao.SenhaEmail);

                    MailMessage mensagemEmail = new MailMessage(new MailAddress(configuracao.EnderecoEmail), new MailAddress(emailDestino))
                    {
                        Body = mensagem.Mensagem,
                        IsBodyHtml = true,
                        Subject = mensagem.Assunto
                    };

                    clienteSmtp.Send(mensagemEmail);
                }
            }
        }

        public void EnviarEmailCodigoInscricao(Inscricao inscricao, string codigo)
        {
            var mensagemEnvio = m_Contexto.RepositorioMensagensEmailPadrao.Obter(inscricao.Evento.Id);
            if (mensagemEnvio != null)
            {
                var tradutor = new TraducaoVariaveisEmail(m_Contexto.RepositorioVariaveisEmailIncricao.ListarTodas(), inscricao);
                tradutor.AoObterValor += (variavel) =>
                {
                    if (variavel.Descricao.ToUpper() == "CODIGO")
                        return codigo;
                    else
                        return "";
                };

                var mensagem = mensagemEnvio.MensagemInscricaoCodigoAcessoAcompanhamento
                    .GerarMensagemTraduzidaVariaveis(tradutor);
                Enviar(inscricao.Evento, mensagem, inscricao.Pessoa.Email);
            }
            else
                throw new Exception("Não há mensagens de email definida para o evento");
        }
    }
}
