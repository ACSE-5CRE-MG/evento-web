using EventoWeb.Nucleo.Negocio.Repositorios;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Usuario
    {
        public Usuario(string login)
        {
            if (login == null)
                throw new ArgumentNullException("Login", "O login do usuário não pode ser nulo.");

            if (login.Trim().Length == 0)
                throw new ArgumentException("O login do usuário não pode ser vazio.");

            Login = login;
        }

        protected Usuario() { }

        public virtual String Login 
        {
            get;
            protected set;
        }
    }
}
