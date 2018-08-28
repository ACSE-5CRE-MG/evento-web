using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum TipoTransacao { Receita, Despesa }

    public class Transacao : EntidadeFinan 
    {
        private string m_OQue;
        private Conta m_QualConta;
        private decimal m_Valor;
        private Categoria m_QualCategoria;
        private Evento m_QualEvento;

        public Transacao(Evento evento, Conta conta, Categoria categoria, DateTime dataHora, String oQue, decimal valor,
            TipoTransacao tipo)
        {
            QualEvento = evento;
            QualConta = conta;
            DataHora = dataHora;
            OQue = oQue;
            Valor = valor;
            Tipo = tipo;
            QualCategoria = categoria;
        }

        protected Transacao() { }

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

        public virtual PessoaComum Quem
        {
            get;
            set;
        }

        public virtual Conta QualConta
        {
            get { return m_QualConta; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("QualConta");

                m_QualConta = value;
            }
        }

        public virtual DateTime DataHora { get; protected set; }

        public virtual String OQue
        {
            get { return m_OQue; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("OQue");

                m_OQue = value;
            }
        }

        public virtual Decimal Valor
        {
            get { return m_Valor; }
            protected set
            {
                if (value <= 0)
                    throw new ArgumentException("O valor deve ser maior que zero.", "Valor");

                m_Valor = value;
            }
        }

        public virtual TipoTransacao Tipo { get; protected set; }

        public virtual Categoria QualCategoria
        {
            get { return m_QualCategoria; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("QualCategoria");

                if (value.QualTransacao != this.Tipo)
                    throw new InvalidOperationException("A categoria deve ser do mesmo tipo do Titulo.");

                m_QualCategoria = value;
            }
        }
    }
}
