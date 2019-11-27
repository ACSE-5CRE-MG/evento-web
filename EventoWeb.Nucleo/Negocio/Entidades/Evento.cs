using EventoWeb.Nucleo.Negocio.Excecoes;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum EnumPublicoEvangelizacao { Todos, TrabalhadoresOuParticipantesTrabalhadores }

    public enum EnumModeloDivisaoSalasEstudo { PorIdadeCidade, PorOrdemEscolhaInscricao }

    public class Evento: Entidade
    {
        private string m_Nome;
        private DateTime m_DataRegistro;
        private Periodo m_PeriodoInscricaoOnLine;
        private Periodo m_PeriodoRealizacaoEvento;
        private EnumPublicoEvangelizacao? m_ConfiguracaoEvangelizacao;
        private int? m_ConfiguracaoTempoSarauMin;
        private EnumModeloDivisaoSalasEstudo? m_ConfiguracaoSalaEstudo;
        private int m_IdadeMinimaInscricaoAdulto;
        private decimal m_ValorInscricaoAdulto;
        private decimal m_ValorInscricaoCrianca;

        protected Evento()
        {
        }

        public Evento(string nome, Periodo periodoInscricaoOnline, Periodo periodoRealizacaoEvento, int idadeMinimaInscricaoAdulto)
        {
            Nome = nome;
            PeriodoInscricaoOnLine = periodoInscricaoOnline;
            PeriodoRealizacaoEvento = periodoRealizacaoEvento;
            IdadeMinimaInscricaoAdulto = idadeMinimaInscricaoAdulto;
            m_DataRegistro = DateTime.Today;
        }

        public virtual string Nome 
        {
            get { return m_Nome; }
            set
            {
                if (value == null)
                    throw new ExcecaoNegocioAtributo("Evento", "Nome", "Nome vazio");

                if (String.IsNullOrEmpty(value))
                    throw new ExcecaoNegocioAtributo("Evento", "Nome", "Nome vazio.");

                m_Nome = value;
            }
        }

        public virtual Periodo PeriodoRealizacaoEvento
        {
            get { return m_PeriodoRealizacaoEvento; }
            set
            {
                if (value == null)
                    throw new ExcecaoNegocioAtributo("Evento", "PeriodoRealizacaoEvento", "PeriodoRealizacaoEvento vazio.");
                m_PeriodoRealizacaoEvento = value;
            }
        }

        public virtual Periodo PeriodoInscricaoOnLine
        {
            get { return m_PeriodoInscricaoOnLine; }
            set
            {
                if (value == null)
                    throw new ExcecaoNegocioAtributo("Evento", "PeriodoInscricaoOnLine", "PeriodoInscricaoOnLine vazio.");

                m_PeriodoInscricaoOnLine = value;
            }
        }

        public virtual DateTime DataRegistro { get { return m_DataRegistro; } }

        public virtual ArquivoBinario Logotipo { get; set; }

        public virtual Boolean TemOficinas { get; set; }

        public virtual Boolean TemDormitorios { get; set; }

        public virtual Boolean TemDepartamentalizacao { get; set; }

        public virtual EnumModeloDivisaoSalasEstudo? ConfiguracaoSalaEstudo
        {
            get => m_ConfiguracaoSalaEstudo;
            set => m_ConfiguracaoSalaEstudo = value;
        }

        public virtual EnumPublicoEvangelizacao? ConfiguracaoEvangelizacao
        {
            get => m_ConfiguracaoEvangelizacao;
            set => m_ConfiguracaoEvangelizacao = value;
        }

        public virtual int? ConfiguracaoTempoSarauMin
        {
            get => m_ConfiguracaoTempoSarauMin;
            set
            {
                if (value != null && value <= 0)
                    throw new ExcecaoNegocioAtributo("Evento", "ConfiguracaoTempoSarauMin", "Tempo de duração do Sarau deve ser maior que zero");

                m_ConfiguracaoTempoSarauMin = value;
            }
        }
        public virtual int IdadeMinimaInscricaoAdulto 
        {
            get => m_IdadeMinimaInscricaoAdulto;
            set
            {
                if (value <= 0)
                    throw new ExcecaoNegocioAtributo("Evento", "IdadeMinimaInscricaoAdulto", "IdadeMinimaInscricaoAdulto Deve ser maior que zero.");
                m_IdadeMinimaInscricaoAdulto = value;
            }
        }

        public virtual decimal ValorInscricaoAdulto 
        {
            get => m_ValorInscricaoAdulto;
            set
            {
                if (value < 0)
                    throw new ExcecaoNegocioAtributo("Evento", "ValorInscricaoAdulto", "Valor da inscrição Adulto deve ser maior ou igual a zero.");
                m_ValorInscricaoAdulto = value;
            }
        }

        public virtual decimal ValorInscricaoCrianca
        {
            get => m_ValorInscricaoCrianca;
            set
            {
                if (value < 0)
                    throw new ExcecaoNegocioAtributo("Evento", "ValorInscricaoCrianca", "Valor da inscrição Criança deve ser maior ou igual a zero.");
                m_ValorInscricaoCrianca = value;
            }
        }
    }
}
