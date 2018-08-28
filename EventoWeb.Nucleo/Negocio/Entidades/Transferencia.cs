using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Transferencia : EntidadeFinan
    {
        private Evento m_QualEvento;
        public Transferencia(Evento evento, Transacao transacaoDebito, Transacao transacaoCredito)
        {
            QualEvento = evento;

            if (transacaoDebito == null)
                throw new ArgumentNullException("transacaoDebito");

            if (transacaoDebito.Tipo != TipoTransacao.Despesa)
                throw new ArgumentException("A transação de débito deve ser do tipo Despesa.");

            if (transacaoCredito == null)
                throw new ArgumentNullException("transacaoCredito");

            if (transacaoCredito.Tipo != TipoTransacao.Receita)
                throw new ArgumentException("A transação de crédito deve ser do tipo Receita.");

            if (transacaoDebito.QualEvento != transacaoCredito.QualEvento)
                throw new ArgumentException("A transação de débito e crédito devem ser do mesmo evento.");

            if (transacaoDebito.Valor != transacaoCredito.Valor)
                throw new ArgumentException("A transação de débito e crédito devem ter o mesmo valor.");

            if (transacaoDebito.DataHora != transacaoCredito.DataHora)
                throw new ArgumentException("A transação de débito e crédito devem ter a mesma data.");

            Data = transacaoDebito.DataHora;
            TransacaoDebito = transacaoDebito;
            TransacaoCredito = transacaoCredito;
        }

        protected Transferencia() { }

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

        public virtual DateTime Data { get; protected set; }
        public virtual Transacao TransacaoDebito { get; protected set; }
        public virtual Transacao TransacaoCredito { get; protected set; }
    }
}
