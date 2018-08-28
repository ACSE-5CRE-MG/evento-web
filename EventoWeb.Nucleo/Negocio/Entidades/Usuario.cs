using EventoWeb.Nucleo.Negocio.Repositorios;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Usuario
    {
        private string mLogin;
        private AUsuarios mRepUsuarios;

        public Usuario(AUsuarios repUsuarios)
        {
            if (repUsuarios == null)
                throw new ArgumentNullException("repUsuarios", "O Repositório de usuários não pode ser nulo.");

            mRepUsuarios = repUsuarios;
        }

        protected Usuario() { }

        public virtual String Login 
        {
            get
            {
                return mLogin;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Login", "O login do usuário não pode ser nulo.");

                if (value.Trim().Length == 0)
                    throw new ArgumentException("O login do usuário não pode ser vazio.");

                if (value != mLogin)
                {
                    if (mRepUsuarios.ObterUsuarioPeloLogin(value) == null)
                        mLogin = value;
                    else
                        throw new Exception(String.Format("O login \"{0}\" já existe.", value));
                }
            }
        }
    }
}
