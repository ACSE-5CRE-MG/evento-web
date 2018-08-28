using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class ParcelaTitulo : EntidadeFinan
    {
        private Titulo m_TituloOrigem;
        private DateTime m_DataRegistrarTransacao;
        private decimal m_Valor;

        public ParcelaTitulo(Titulo titulo, DateTime dataRegistrarTransacao, decimal valor)
        {
            TituloOrigem = titulo;
            DataRegistrarTransacao = dataRegistrarTransacao;
            Registrado = false;
            RegistroTransacao = null;
            Valor = valor;
        }

        protected ParcelaTitulo() { }

        public virtual Transacao RegistroTransacao { get; protected set; }

        public virtual Titulo TituloOrigem 
        {
            get { return m_TituloOrigem; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("TituloOrigem");
                m_TituloOrigem = value;
            }
        }

        public virtual DateTime DataRegistrarTransacao 
        {
            get { return m_DataRegistrarTransacao; }
            set
            {
                if (!TituloOrigem.PodeModificarParcelamento())
                    throw new ArgumentException("Já existem parcelas transacionadas", "DataRegistrarTransacao");

                m_DataRegistrarTransacao = value;
            }
        }

        public virtual bool Registrado { get; protected set; }

        public virtual decimal Valor
        {
            get { return m_Valor; }
            protected set
            {
                if (value <= 0)
                    throw new ArgumentException("O valor deve ser maior que zero.", "Valor");

                m_Valor = value;
            }
        }

        public virtual void RegistrarTransacao(Transacao transacao)
        {
            if (transacao == null)
                throw new ArgumentNullException("transacao");

            if (transacao.Valor != this.Valor)
                throw new InvalidOperationException("A transação deve ter o mesmo valor da Parcela.");

            RegistroTransacao = transacao;
            Registrado = true;

            TituloOrigem.AtualizarSituacao();
        }

        public virtual Transacao EstornarRegistroTransacao()
        {
            if (!Registrado)
                throw new InvalidOperationException("Esta parcela não foi registrada.");

            var transacao = RegistroTransacao;

            RegistroTransacao = null;
            Registrado = false;

            TituloOrigem.AtualizarSituacao();

            return transacao;
        }
    }
}
