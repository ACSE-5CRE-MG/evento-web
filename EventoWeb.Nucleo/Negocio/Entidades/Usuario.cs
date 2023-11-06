using EventoWeb.Nucleo.Negocio.Excecoes;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Usuario
    {
        private string m_Nome;
        private SenhaUsuario m_Senha;

        public Usuario(string login, string nome, SenhaUsuario senha)
        {
            if (login == null)
                throw new ExcecaoNegocioAtributo("Usuario", "Login", "O login do usuário não pode ser nulo.");

            if (login.Trim().Length == 0)
                throw new ExcecaoNegocioAtributo("Usuario", "Login", "O login do usuário não pode ser vazio.");

            Login = login;
            Nome = nome;
            m_Senha = senha ?? throw new ExcecaoNegocioAtributo(nameof(Usuario), nameof(Senha), "A senha não pode ser nula.");
        }

        protected Usuario() { }

        public virtual String Login 
        {
            get;
            protected set;
        }

        public virtual string Nome
        {
            get
            {
                return m_Nome;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ExcecaoNegocioAtributo(nameof(Usuario), nameof(Nome), "O nome não pode ser nulo ou vazio.");

                m_Nome = value;
            }
        }

        public virtual SenhaUsuario Senha
        {
            get
            {
                return m_Senha;
            }
        }

        public virtual bool EhAdministrador { get; set; } = false;
    }
}
