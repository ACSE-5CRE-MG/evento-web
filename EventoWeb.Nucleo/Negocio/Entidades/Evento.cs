﻿using EventoWeb.Nucleo.Negocio.Excecoes;
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
        private ConfiguracaoEvangelizacao m_ConfiguracaoEvangelizacao;
        private ConfiguracaoSarau m_ConfiguracaoSarau;
        private ConfiguracaoSalaEstudo m_ConfiguracaoSalaEstudo;
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

        public virtual ConfiguracaoSalaEstudo ConfiguracaoSalaEstudo
        {
            get => m_ConfiguracaoSalaEstudo;
            set => m_ConfiguracaoSalaEstudo = value;
        }

        public virtual ConfiguracaoEvangelizacao ConfiguracaoEvangelizacao
        {
            get => m_ConfiguracaoEvangelizacao;
            set => m_ConfiguracaoEvangelizacao = value;
        }

        public virtual ConfiguracaoSarau ConfiguracaoSarau
        {
            get => m_ConfiguracaoSarau;
            set => m_ConfiguracaoSarau = value;
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

    public class ConfiguracaoEvangelizacao
    {
        private EnumPublicoEvangelizacao m_Publico;

        public ConfiguracaoEvangelizacao(EnumPublicoEvangelizacao publico)
        {
            m_Publico = publico;
        }

        protected ConfiguracaoEvangelizacao() { }

        public virtual EnumPublicoEvangelizacao Publico { get => m_Publico; }
    }

    public class ConfiguracaoSarau
    {
        private int m_TempoDuracaoMin;

        public ConfiguracaoSarau(int tempoDuracaoMin)
        {
            if (tempoDuracaoMin <= 0)
                throw new ExcecaoNegocioAtributo("ConfiguracaoSarau", "tempoDuracaoMin", "Tempo de duração deve ser maior que zero");

            m_TempoDuracaoMin = tempoDuracaoMin;
        }

        protected ConfiguracaoSarau() { }

        public int TempoDuracaoMin { get => m_TempoDuracaoMin; }
    }

    public class ConfiguracaoSalaEstudo
    {
        private EnumModeloDivisaoSalasEstudo m_ModeloDivisao;

        public ConfiguracaoSalaEstudo(EnumModeloDivisaoSalasEstudo modeloDivisao)
        {
            m_ModeloDivisao = modeloDivisao;
        }

        protected ConfiguracaoSalaEstudo() { }

        public virtual EnumModeloDivisaoSalasEstudo ModeloDivisao { get => m_ModeloDivisao; }
    }
}
