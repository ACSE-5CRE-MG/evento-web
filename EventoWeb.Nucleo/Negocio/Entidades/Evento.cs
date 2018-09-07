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
        private string m_Nome;
        private DateTime m_DataInicioInscricao;
        private DateTime m_DataFimInscricao;
        private SituacaoEvento m_Situacao;
        private DateTime m_DataRegistro;
        private ConfiguracaoEmail m_ConfiguracaoEmail;
        private decimal m_ValorInscricao;
        private DateTime m_DataFimEvento;
        private DateTime m_DataInicioEvento;
        private EnumPublicoEvangelizacao? m_PublicoEvangelizacao;
        private bool m_TemEvangelizacao;
        private int? m_TempoDuracaoSarauMin;
        private bool m_TemSarau;
        private bool m_TemSalasEstudo;
        private EnumModeloDivisaoSalasEstudo? m_ModeloDivisaoSalasEstudo;

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

            m_DataRegistro = DateTime.Today;
            m_Situacao = SituacaoEvento.Aberto;
            m_ConfiguracaoEmail = new ConfiguracaoEmail();
        }

        public virtual string Nome 
        {
            get { return m_Nome; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Nome");

                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Nome vazio.", "Nome");

                m_Nome = value;
            }
        }

        public virtual void DefinirDataInscricao(DateTime dataInicial, DateTime dataFinal)
        {
            if (dataInicial >= dataFinal)
                throw new ArgumentException("Data inicial da inscrição dever ser menor que a final.", "dataInicial");

            m_DataInicioInscricao = dataInicial;
            m_DataFimInscricao = dataFinal;
        }

        public virtual void DefinirDataEvento(DateTime dataInicial, DateTime dataFinal)
        {
            if (dataInicial >= dataFinal)
                throw new ArgumentException("Data inicial do evento dever ser menor que a final.", "dataInicial");

            m_DataInicioEvento = dataInicial;
            m_DataFimEvento = dataFinal;
        }

        public virtual DateTime DataInicioInscricao { get { return m_DataInicioInscricao; } }

        public virtual DateTime DataFimInscricao { get { return m_DataFimInscricao; } }

        public virtual DateTime DataInicioEvento { get { return m_DataInicioEvento; } }

        public virtual DateTime DataFimEvento { get { return m_DataFimEvento; } }

        public virtual SituacaoEvento Situacao { get { return m_Situacao; } }

        public virtual DateTime DataRegistro { get { return m_DataRegistro; } }

        public virtual String Logotipo { get; set; }

        public virtual Boolean TemDepartamentalizacao { get; set; }

        public virtual Boolean TemSalasEstudo { get { return m_TemSalasEstudo; } }

        public virtual EnumModeloDivisaoSalasEstudo? ModeloDivisaoSalasEstudo { get { return m_ModeloDivisaoSalasEstudo; } }

        public virtual void NaoUtilizaSalasEstudo()
        {
            m_TemSalasEstudo = false;
            m_ModeloDivisaoSalasEstudo = null;
        }

        public virtual void UtilizaSalasEstudo(EnumModeloDivisaoSalasEstudo modeloDivisao)
        {
            m_TemSalasEstudo = true;
            m_ModeloDivisaoSalasEstudo = modeloDivisao;
        }

        public virtual Boolean TemOficinas { get; set; }

        public virtual Boolean TemDormitorios { get; set; }

        public virtual Boolean TemEvangelizacao { get { return m_TemEvangelizacao; } }

        public virtual EnumPublicoEvangelizacao? PublicoEvangelizacao { get { return m_PublicoEvangelizacao; } }

        public virtual void ConfigurarEvangelizacao(bool temEvangelizacao, EnumPublicoEvangelizacao? publicoEvangelizacao)
        {
            if (temEvangelizacao && publicoEvangelizacao == null)
                throw new ArgumentException("Informe o público da evangelização");

            m_TemEvangelizacao = temEvangelizacao;
            m_PublicoEvangelizacao = publicoEvangelizacao;
        }

        public virtual Boolean TemSarau { get { return m_TemSarau; } }

        public virtual int? TempoDuracaoSarauMin { get { return m_TempoDuracaoSarauMin; } }

        public virtual void ConfigurarSarau(bool temSarau, int? tempoDuracaoSarauMin)
        {
            if (temSarau && tempoDuracaoSarauMin == null)
                throw new ArgumentException("Informe o tempo de duração do Sarau");

            m_TemSarau = temSarau;
            m_TempoDuracaoSarauMin = tempoDuracaoSarauMin;
        }

        public virtual ConfiguracaoEmail ConfiguracaoEmail
        {
            get { return m_ConfiguracaoEmail; }
        }

        public virtual Decimal ValorInscricao
        {
            get { return m_ValorInscricao; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("O valor da inscrição deve ser maior ou igual a zero.");

                m_ValorInscricao = value;
            }
        }

        public virtual void concluir()
        {
            if (m_Situacao == SituacaoEvento.Aberto)
                throw new Exception("Não se pode concluir um evento na situação ABERTO.");

            if (m_Situacao != SituacaoEvento.Concluido)
                m_Situacao = SituacaoEvento.Concluido;
        }

        public virtual bool EstaAbertoNestaData(DateTime data) 
        {
                return Situacao != SituacaoEvento.Concluido && (data >= DataInicioInscricao && data <= DataFimEvento);
        }
    }
}
