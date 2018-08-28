using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Conta : EntidadeFinan
    {
        private string m_Descricao;
        private Evento m_QualEvento;

        public Conta(Evento evento, String descricao, Decimal saldoInicial = 0)
        {
            QualEvento = evento;
            Descricao = descricao;
            SaldoInicial = saldoInicial;
        }

        protected Conta() { }

        public virtual Evento QualEvento
        {
            get { return m_QualEvento; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Evento");

                m_QualEvento = value;
            }
        }

        public virtual String Descricao 
        {
            get { return m_Descricao; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Descricao");

                m_Descricao = value;
            }
        }

        public virtual Decimal SaldoInicial { get; set; }
    }
}
