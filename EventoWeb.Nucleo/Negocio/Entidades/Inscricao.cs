using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum TipoSituacaoPagamento { Pago, Pagar, Isento }

    public abstract class Inscricao: Entidade
    {
        private Pessoa mPessoa;
        private Evento mEvento;
        private DateTime mDataRecebimento;
        private bool mDormeEvento;

        public Inscricao(Pessoa pessoa, Evento evento, DateTime dataRecebimento)
        {
            if (evento == null)
                throw new ArgumentNullException("Evento");

            if (pessoa == null)
                throw new ArgumentNullException("Pessoa");

            if (!EhValidaIdade(pessoa.CalcularIdadeEmAnos(evento.DataInicioEvento)))
                throw new ArgumentException("A idade da pessoa é inválida para este tipo de inscrição.");

            mPessoa = pessoa;
            mEvento = evento;
            DataRecebimento = dataRecebimento;

            IsentarInscricao();
        }

        protected Inscricao() { }

        public virtual Pessoa Pessoa { get { return mPessoa; } }
        public virtual Evento Evento { get { return mEvento; } }
        
        public virtual DateTime DataRecebimento 
        { 
            get { return mDataRecebimento; }
            set
            {
                if (!mEvento.EstaAbertoNestaData(value))
                    throw new ArgumentException("A data de recebimento deve estar dentro do período de inscrição");

                mDataRecebimento = value;
            }
        }

        public virtual Boolean DormeEvento
        {
            get { return mDormeEvento; }
            set { mDormeEvento = value; }
        }

        public virtual Titulo TituloPagar { get; protected set; }

        public virtual Transacao TransacaoPagamento { get; protected set; }

        public virtual TipoSituacaoPagamento SituacaoFinanceiro { get; protected set; }
        public virtual bool ConfirmadoNoEvento { get; set; }

        public virtual void PagarDepois(Titulo titulo)
        {
            if (titulo == null)
                throw new ArgumentNullException("titulo");

            if (TituloPagar != null)
                throw new ArgumentException("Esta inscrição já foi vinculada a um título.");

            if (SituacaoFinanceiro == TipoSituacaoPagamento.Pago)
                throw new ArgumentException("Esta inscrição já foi paga.");

            TituloPagar = titulo;
            SituacaoFinanceiro = TipoSituacaoPagamento.Pagar;
        }

        public virtual void PagarAgora(Transacao transacao)
        {
            if (transacao == null)
                throw new ArgumentNullException("transacao");

            if (TransacaoPagamento != null)
                throw new ArgumentException("Esta inscrição já foi vinculada a uma Transação de Pagamento.");

            TransacaoPagamento = transacao;
            SituacaoFinanceiro = TipoSituacaoPagamento.Pago;
        }

        public virtual void IsentarInscricao()
        {
            TituloPagar = null;
            TransacaoPagamento = null;
            SituacaoFinanceiro = TipoSituacaoPagamento.Isento;
        }

        public abstract Boolean EhValidaIdade(int idade);
    }
}
