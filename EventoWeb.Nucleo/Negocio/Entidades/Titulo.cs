using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum TipoSituacaoTitulo { Liquidado, ParcialmenteLiquidado, Aberto }
    public class DadosParcelamento 
    {
        public DateTime Data { get; set; }
        public Decimal Valor { get; set; }
    }

    public class Titulo : EntidadeFinan
    {
        private IList<ParcelaTitulo> m_Parcelas;
        private PessoaComum m_Quem;
        private Categoria m_QualCategoria;
        private Evento m_QualEvento;

        public Titulo(Evento evento, TipoTransacao tipo, PessoaComum quem, Categoria categoria, Decimal valor, IEnumerable<DadosParcelamento> parcelas)
        {
            QualEvento = evento;
            Tipo = tipo;
            QualCategoria = categoria;
            Quem = quem;

            m_Parcelas = new List<ParcelaTitulo>();

            AlterarValor(valor, parcelas);

            DataCriado = DateTime.Now;
            Situacao = TipoSituacaoTitulo.Aberto;
        }

        protected Titulo() { }

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

        public virtual decimal Valor { get; protected set; }

        public virtual TipoSituacaoTitulo Situacao { get; protected set; }

        public virtual DateTime DataCriado { get; protected set; }

        public virtual string Descricao { get; set; }

        public virtual PessoaComum Quem 
        {
            get { return m_Quem; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Quem");
                m_Quem = value;
            }
        }

        public virtual IEnumerable<ParcelaTitulo> Parcelas { get { return m_Parcelas; } }

        public virtual void AlterarValor(Decimal novoValor, IEnumerable<DadosParcelamento> parcelas)
        {
            if (novoValor <= 0)
                throw new ArgumentException("O valor deve ser maior que zero.", "novoValor");

            var valorAtual = Valor;
            Valor = novoValor;

            try
            {
                Parcelar(parcelas);
            }
            catch (Exception ex)
            {
                Valor = valorAtual;
                throw ex;
            }
        }

        public virtual void Parcelar(IEnumerable<DadosParcelamento> parcelas)
        {
            if (parcelas == null || parcelas.Count() == 0)
                throw new ArgumentNullException("parcelas");

            if (parcelas.Sum(x => x.Valor) != Valor)
                throw new ArgumentException("O valor das parcelas deve ser igual ao do titulo.");

            if (!PodeModificarParcelamento())
                throw new ArgumentException("Já existem parcelas transacionadas, por isso é impossível mudar o parcelamento do titulo.");

            m_Parcelas.Clear();

            foreach (var dado in parcelas)
            {
                var parcelaTitulo = new ParcelaTitulo(this, dado.Data, dado.Valor);
                m_Parcelas.Add(parcelaTitulo);
            }
        }

        public virtual bool PodeModificarParcelamento()
        {
            return Parcelas == null || (Parcelas != null && Parcelas.Count(x => x.Registrado) == 0);
        }

        public virtual void AtualizarSituacao()
        {
            int numeroParcelas = Parcelas.Count();
            int numeroParcelasLiquidadas = Parcelas.Count(x => x.Registrado);

            if (numeroParcelas == numeroParcelasLiquidadas)
                Situacao = TipoSituacaoTitulo.Liquidado;
            else if (numeroParcelasLiquidadas == 0)
                Situacao = TipoSituacaoTitulo.Aberto;
            else
                Situacao = TipoSituacaoTitulo.ParcialmenteLiquidado;
        }
    }
}
