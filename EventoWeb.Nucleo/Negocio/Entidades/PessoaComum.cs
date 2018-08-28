using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public abstract class PessoaComum: Entidade
    {
        private string mNome;
        public PessoaComum(string nome)
        {
            Nome = nome;
        }

        protected PessoaComum() { }

        public virtual string Nome
        {
            get
            {
                return mNome;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Nome");

                if (value.Trim().Length == 0)
                    throw new ArgumentException("Nome não pode ser vazio.", "Nome");

                mNome = value;
            }
        }

        public virtual String NomeConhecido { get; set; }

        public virtual string TelefoneFixo { get; set; }

        public virtual string Celular { get; set; }

        public virtual string Email { get; set; }
    }
}
