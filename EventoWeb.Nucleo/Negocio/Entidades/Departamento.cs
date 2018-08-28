using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Departamento: Entidade
    {
        private Evento mEvento;
        private string mNome;

        public Departamento(Evento evento, String nome)
        {
            Evento = evento;

            Nome = nome;
        }

        protected Departamento() { }

        public virtual Evento Evento
        {
            get { return mEvento; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Evento", "Evento não pode ser nulo.");

                if (!value.EstaAbertoNestaData(DateTime.Now))
                    throw new ArgumentException("O evento já foi encerrado.", "evento");

                if (!value.TemDepartamentalizacao)
                    throw new InvalidOperationException("Este evento não está configurado para ter departamentos.");

                mEvento = value;
            }
        }

        public virtual String Nome
        {
            get { return mNome; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Nome");

                mNome = value;
            }
        }
    }
}
