using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;
using System.Net;
using System.Net.Mail;

namespace EventoWeb.Nucleo.Persistencia.Comunicacao
{
    public class ServicoEmail : AServicoEmail
    {
        public override void Enviar(Email email)
        {
            if (Configuracao == null)
                throw new ExcecaoNegocio("ServicoEmail", "Configuração de email precisa ser informada.");

            using (SmtpClient clienteSmtp = new SmtpClient(Configuracao.ServidorEmail, Configuracao.PortaServidor.Value))
            {
                clienteSmtp.EnableSsl = Configuracao.TipoSeguranca.Value == TipoSegurancaEmail.SSL;
                clienteSmtp.Credentials = new NetworkCredential(Configuracao.UsuarioEmail, Configuracao.SenhaEmail);

                MailMessage mensagemEmail = new MailMessage(new MailAddress(Configuracao.EnderecoEmail), new MailAddress(email.Endereco))
                {
                    Body = email.Conteudo,
                    IsBodyHtml = true,
                    Subject = email.Assunto,                    
                };
                mensagemEmail.ReplyToList.Add(new MailAddress(email.EnderecoResposta));

                clienteSmtp.Send(mensagemEmail);
            }
        }
    }
}
