using System;
using System.Net.Mail;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum TipoSegurancaEmail { SSL, Nenhuma }

    public class ConfiguracaoEmail
    {
        private String mEnderecoEmail;
        private String mUsuarioEmail;
        private String mSenhaEmail;
        private String mServidorEmail;
        private int? mPortaServidor;
        private TipoSegurancaEmail? mTipoSeguranca;
        private String mTituloEmailConfirmacaoInscricao;
        private String mMensagemEmailConfirmacaoInscricao;

        public virtual String EnderecoEmail
        {
            get { return mEnderecoEmail; }
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
                mEnderecoEmail = value; 
            }
        }

        public virtual String UsuarioEmail
        {
            get { return mUsuarioEmail; }
            set { mUsuarioEmail = value; }
        }

        public virtual String SenhaEmail
        {
            get { return mSenhaEmail; }
            set { mSenhaEmail = value; }
        }

        public virtual String ServidorEmail
        {
            get { return mServidorEmail; }
            set { mServidorEmail = value; }
        }

        public virtual int? PortaServidor
        {
            get { return mPortaServidor; }
            set { mPortaServidor = value; }
        }

        public virtual TipoSegurancaEmail? TipoSeguranca
        {
            get { return mTipoSeguranca; }
            set { mTipoSeguranca = value; }
        }

        public virtual String TituloEmailConfirmacaoInscricao
        {
            get { return mTituloEmailConfirmacaoInscricao; }
            set { mTituloEmailConfirmacaoInscricao = value; }
        }

        public virtual String MensagemEmailConfirmacaoInscricao
        {
            get { return mMensagemEmailConfirmacaoInscricao; }
            set { mMensagemEmailConfirmacaoInscricao = value; }
        }

        public virtual Boolean ConfiguracaoInformada
        {
            get
            {
                return !String.IsNullOrEmpty(mEnderecoEmail) &&
                       !String.IsNullOrEmpty(mSenhaEmail) &&
                       !String.IsNullOrEmpty(mServidorEmail) &&
                       !String.IsNullOrEmpty(mUsuarioEmail) &&
                       mPortaServidor != null &&
                       mTipoSeguranca != null;
            }
        }
    }
}
