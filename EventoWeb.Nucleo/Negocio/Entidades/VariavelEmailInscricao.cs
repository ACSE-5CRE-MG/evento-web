using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class VariavelEmailInscricao
    {
        public VariavelEmailInscricao(string descricao, string variavel, string campoInscricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ExcecaoNegocioAtributo("VariavelEmailInscricao", "descricao", "Descrição deve ser informada");

            if (string.IsNullOrWhiteSpace(variavel))
                throw new ExcecaoNegocioAtributo("VariavelEmailInscricao", "variavel", "Variável deve ser informada");

            if (string.IsNullOrWhiteSpace(campoInscricao))
                throw new ExcecaoNegocioAtributo("VariavelEmailInscricao", "campo", "Campo deve ser informado");

            Descricao = descricao;
            Variavel = variavel;
            CampoInscricao = campoInscricao;
        }

        protected VariavelEmailInscricao() { }

        public virtual string Descricao { get; protected set; }
        public virtual string Variavel { get; protected set; }
        public virtual string CampoInscricao { get; protected set; }
    }
}
