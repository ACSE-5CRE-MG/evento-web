using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum SexoPessoa { Masculino, Feminino }

    public class Pessoa: PessoaComum
    {
        private DateTime mDataNascimento;
        private Endereco mEndereco;
        private Boolean mEhVegetariano;
        private String mMedicamentosUsados;
        private SexoPessoa mSexo;
        private String mInstituicoesEspiritasFrequenta;
        private String mObservacoes;

        protected Pessoa()
        {
        }

        public Pessoa(string nome, Endereco endereco, DateTime dataNascimento, SexoPessoa sexo)
            :base(nome)
        {
            if (endereco == null)
                throw new ArgumentNullException("Endereco");
            mEndereco = endereco;
            DataNascimento = dataNascimento;
            mSexo = sexo;
        }

        public virtual DateTime DataNascimento
        {
            get
            {
                return mDataNascimento;
            }

            set
            {
                DateTime dataHoje = DateTime.Now;
                dataHoje = dataHoje.AddMilliseconds(dataHoje.Millisecond * -1);
                dataHoje = dataHoje.AddSeconds(dataHoje.Second * -1);

                if (value > dataHoje)
                    throw new ArgumentException("Data deve ser menor que a data atual do sistema.", "DataNascimento");

                mDataNascimento = value;
            }
        }

        public virtual Endereco Endereco { get { return mEndereco; } }

        public virtual Boolean EhVegetariano 
        {
            get { return mEhVegetariano; }
            set { mEhVegetariano = value; }
        }

        public virtual Boolean EhDiabetico { get; set; }

        public virtual Boolean UsaAdocanteDiariamente { get; set; }

        public virtual String AlergiaAlimentos { get; set; }

        public virtual String TiposCarneNaoCome { get; set; }

        public virtual String MedicamentosControlados { get; set; }

        public virtual String MedicamentosUsados
        {
            get { return mMedicamentosUsados; }
            set { mMedicamentosUsados = value; }
        }      

        public virtual SexoPessoa Sexo
        {
            get { return mSexo; }
            set { mSexo = value; }
        }

        public virtual String InstituicoesEspiritasFrequenta
        {
            get { return mInstituicoesEspiritasFrequenta; }
            set { mInstituicoesEspiritasFrequenta = value; }
        }

        public virtual String Observacoes
        {
            get { return mObservacoes; }
            set { mObservacoes = value; }
        }

        public virtual int CalcularIdadeEmAnos(DateTime dataAtual)
        {
            if (dataAtual < mDataNascimento)
                throw new ArgumentException("A data de comparação deve ser maior ou igual a do nascimento.", "dataAtual");

            return (dataAtual.Year - mDataNascimento.Year - 1) +
                    (((dataAtual.Month > mDataNascimento.Month) ||
                    ((dataAtual.Month == mDataNascimento.Month) && (dataAtual.Day >= mDataNascimento.Day))) ? 1 : 0);
        }
    }
}
