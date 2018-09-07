using System;
using System.Net.Mail;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum TipoSegurancaEmail { SSL, Nenhuma }

    public class ConfiguracaoEmail
    {
        private String m_EnderecoEmail;
        private String m_UsuarioEmail;
        private String m_SenhaEmail;
        private String m_ServidorEmail;
        private int? m_PortaServidor;
        private TipoSegurancaEmail? m_TipoSeguranca;
        private String m_TituloEmailConfirmacaoInscricao;
        private String m_MensagemEmailConfirmacaoInscricao;

        public virtual String EnderecoEmail
        {
            get { return m_EnderecoEmail; }
            set 
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        new MailAddress(value);
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("O endereço de email informado não é válido.", "EnderecoEmail");
                    }
                }
                m_EnderecoEmail = value; 
            }
        }

        public virtual String UsuarioEmail
        {
            get { return m_UsuarioEmail; }
            set { m_UsuarioEmail = value; }
        }

        public virtual String SenhaEmail
        {
            get { return m_SenhaEmail; }
            set { m_SenhaEmail = value; }
        }

        public virtual String ServidorEmail
        {
            get { return m_ServidorEmail; }
            set { m_ServidorEmail = value; }
        }

        public virtual int? PortaServidor
        {
            get { return m_PortaServidor; }
            set { m_PortaServidor = value; }
        }

        public virtual TipoSegurancaEmail? TipoSeguranca
        {
            get { return m_TipoSeguranca; }
            set { m_TipoSeguranca = value; }
        }

        public virtual String TituloEmailConfirmacaoInscricao
        {
            get { return m_TituloEmailConfirmacaoInscricao; }
            set { m_TituloEmailConfirmacaoInscricao = value; }
        }

        public virtual String MensagemEmailConfirmacaoInscricao
        {
            get { return m_MensagemEmailConfirmacaoInscricao; }
            set { m_MensagemEmailConfirmacaoInscricao = value; }
        }

        public virtual Boolean ConfiguracaoInformada
        {
            get
            {
                return !String.IsNullOrEmpty(m_EnderecoEmail) &&
                       !String.IsNullOrEmpty(m_SenhaEmail) &&
                       !String.IsNullOrEmpty(m_ServidorEmail) &&
                       !String.IsNullOrEmpty(m_UsuarioEmail) &&
                       m_PortaServidor != null &&
                       m_TipoSeguranca != null;
            }
        }
    }
}
