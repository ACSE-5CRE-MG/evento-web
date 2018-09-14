using EventoWeb.Nucleo.Negocio.Excecoes;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Usuario
    {
        public Usuario(string login)
        {
            if (login == null)
                throw new ExcecaoNegocioAtributo("Usuario", "Login", "O login do usuário não pode ser nulo.");

            if (login.Trim().Length == 0)
                throw new ExcecaoNegocioAtributo("Usuario", "Login", "O login do usuário não pode ser vazio.");

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
