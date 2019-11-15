using System;
using System.Collections.Generic;
using System.Text;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;

namespace EventoWeb.Nucleo.Aplicacao.Comunicacao
{
    public class Email
    {
        public string Endereco { get; set; }
        public string Assunto { get; set; }
        public string Conteudo { get; set; }
    }

    public abstract class AServicoEmail
    {
        private ConfiguracaoEmail m_Configuracao;

        public ConfiguracaoEmail Configuracao 
        {
            get => m_Configuracao;
            set
            {
                m_Configuracao = value ?? throw new ExcecaoNegocio("AServicoEmail", "Configuração de email precisa ser informada.");
            }
        }

        public abstract void Enviar(Email email);
    }
}
