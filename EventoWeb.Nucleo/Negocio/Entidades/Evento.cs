using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum SituacaoEvento { Aberto, EmAndamento, Concluido }

    public enum EnumPublicoEvangelizacao { Todos, TrabalhadoresOuParticipantesTrabalhadores }

    public enum EnumModeloDivisaoSalasEstudo { PorIdadeCidade, PorOrdemEscolhaInscricao }

    public class Evento: Entidade
    {
        private string mNome;
        private DateTime mDataInicioInscricao;
        private DateTime mDataFimInscricao;
        private SituacaoEvento mSituacao;
        private DateTime mDataRegistro;
        private ConfiguracaoEmail mConfiguracaoEmail;
        private decimal mValorInscricao;
        private DateTime mDataFimEvento;
        private DateTime mDataInicioEvento;
        private EnumPublicoEvangelizacao? mPublicoEvangelizacao;
        private bool mTemEvangelizacao;
        private int? mTempoDuracaoSarauMin;
        private bool mTemSarau;
        private bool mTemSalasEstudo;
        private EnumModeloDivisaoSalasEstudo? mModeloDivisaoSalasEstudo;

        protected Evento()
        {
        }

        public Evento(string nome, DateTime dataInicioInscricao, DateTime dataFimInscricao,
            DateTime dataInicioEvento, DateTime dataFimEvento, Decimal valorInscricao)
        {
            Nome = nome;
            ValorInscricao = valorInscricao;
            DefinirDataInscricao(dataInicioInscricao, dataFimInscricao);
            DefinirDataEvento(dataInicioEvento, dataFimEvento);

            mDataRegistro = DateTime.Today;
            mSituacao = SituacaoEvento.Aberto;
            mConfiguracaoEmail = new ConfiguracaoEmail();
        }

        public virtual string Nome 
        {
            get { return mNome; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Nome");

                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Nome vazio.", "Nome");

                mNome = value;
            }
        }

        public virtual void DefinirDataInscricao(DateTime dataInicial, DateTime dataFinal)
        {
            if (dataInicial >= dataFinal)
                throw new ArgumentException("Data inicial da inscrição dever ser menor que a final.", "dataInicial");

            mDataInicioInscricao = dataInicial;
            mDataFimInscricao = dataFinal;
        }

        public virtual void DefinirDataEvento(DateTime dataInicial, DateTime dataFinal)
        {
            if (dataInicial >= dataFinal)
                throw new ArgumentException("Data inicial do evento dever ser menor que a final.", "dataInicial");

            mDataInicioEvento = dataInicial;
            mDataFimEvento = dataFinal;
        }

        public virtual DateTime DataInicioInscricao { get { return mDataInicioInscricao; } }

        public virtual DateTime DataFimInscricao { get { return mDataFimInscricao; } }

        public virtual DateTime DataInicioEvento { get { return mDataInicioEvento; } }

        public virtual DateTime DataFimEvento { get { return mDataFimEvento; } }

        public virtual SituacaoEvento Situacao { get { return mSituacao; } }

        public virtual DateTime DataRegistro { get { return mDataRegistro; } }

        public virtual String Logotipo { get; set; }

        public virtual Boolean TemDepartamentalizacao { get; set; }

        public virtual Boolean TemSalasEstudo { get { return mTemSalasEstudo; } }

        public virtual EnumModeloDivisaoSalasEstudo? ModeloDivisaoSalasEstudo { get { return mModeloDivisaoSalasEstudo; } }

        public virtual void NaoUtilizaSalasEstudo()
        {
            mTemSalasEstudo = false;
            mModeloDivisaoSalasEstudo = null;
        }

        public virtual void UtilizaSalasEstudo(EnumModeloDivisaoSalasEstudo modeloDivisao)
        {
            mTemSalasEstudo = true;
            mModeloDivisaoSalasEstudo = modeloDivisao;
        }

        public virtual Boolean TemOficinas { get; set; }

        public virtual Boolean TemDormitorios { get; set; }

        public virtual Boolean TemEvangelizacao { get { return mTemEvangelizacao; } }

        public virtual EnumPublicoEvangelizacao? PublicoEvangelizacao { get { return mPublicoEvangelizacao; } }

        public virtual void ConfigurarEvangelizacao(bool temEvangelizacao, EnumPublicoEvangelizacao? publicoEvangelizacao)
        {
            if (temEvangelizacao && publicoEvangelizacao == null)
                throw new ArgumentException("Informe o público da evangelização");

            mTemEvangelizacao = temEvangelizacao;
            mPublicoEvangelizacao = publicoEvangelizacao;
        }

        public virtual Boolean TemSarau { get { return mTemSarau; } }

        public virtual int? TempoDuracaoSarauMin { get { return mTempoDuracaoSarauMin; } }

        public virtual void ConfigurarSarau(bool temSarau, int? tempoDuracaoSarauMin)
        {
            if (temSarau && tempoDuracaoSarauMin == null)
                throw new ArgumentException("Informe o tempo de duração do Sarau");

            mTemSarau = temSarau;
            mTempoDuracaoSarauMin = tempoDuracaoSarauMin;
        }

        public virtual ConfiguracaoEmail ConfiguracaoEmail
        {
            get { return mConfiguracaoEmail; }
        }

        public virtual Decimal ValorInscricao
        {
            get { return mValorInscricao; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("O valor da inscrição deve ser maior ou igual a zero.");

                mValorInscricao = value;
            }
        }

        public virtual void concluir()
        {
            if (mSituacao == SituacaoEvento.Aberto)
                throw new Exception("Não se pode concluir um evento na situação ABERTO.");

            if (mSituacao != SituacaoEvento.Concluido)
                mSituacao = SituacaoEvento.Concluido;
        }

        public virtual bool EstaAbertoNestaData(DateTime data) 
        {
                return Situacao != SituacaoEvento.Concluido && (data >= DataInicioInscricao && data <= DataFimEvento);
        }
    }
}
