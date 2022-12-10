using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Comunicacao
{
    public class ServicoEmail : AServicoEmail
    {
        public override void Enviar(Email email)
        {
            if (Configuracao == null)
                throw new ExcecaoNegocio("ServicoEmail", "Configuração de email precisa ser informada.");

            using var clientHttp = new HttpClient() { BaseAddress = new Uri("https://api.sendinblue.com") };
            clientHttp.DefaultRequestHeaders.Accept.Clear();
            clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clientHttp.DefaultRequestHeaders.Add("api-key", "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");
            var resultado = clientHttp.PostAsync("v3/smtp/email",
                new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        sender = new
                        {
                            name = Configuracao.EnderecoEmail,
                            email = Configuracao.EnderecoEmail
                        },
                        to = new[]
                        {
                            new
                            {
                                name=email.Endereco,
                                email=email.Endereco
                            }
                        },
                        subject = email.Assunto,
                        htmlContent = email.Conteudo
                    }),
                    Encoding.UTF8, "application/json")).Result;
        }

        /*public override void Enviar(Email email)
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
                mensagemEmail.ReplyToList.Add(new MailAddress(Configuracao.EnderecoEmail));

                clienteSmtp.Send(mensagemEmail);
            }
        }*/
    }
}
