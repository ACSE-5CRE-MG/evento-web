using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public abstract class AVariavelEmailInscricao
    {
        public AVariavelEmailInscricao(string descricao, string variavel)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ExcecaoNegocioAtributo("VariavelEmailInscricao", "descricao", "Descrição deve ser informada");

            if (string.IsNullOrWhiteSpace(variavel))
                throw new ExcecaoNegocioAtributo("VariavelEmailInscricao", "variavel", "Variável deve ser informada");

            Descricao = descricao;
            Variavel = variavel;
        }

        public virtual string Descricao { get; protected set; }
        public virtual string Variavel { get; protected set; }
    }

    public class VariavelEmailInscricao: AVariavelEmailInscricao
    {
        public VariavelEmailInscricao(string descricao, string variavel, string campoInscricao)
            : base(descricao, variavel)
        {
            if (string.IsNullOrWhiteSpace(campoInscricao))
                throw new ExcecaoNegocioAtributo("VariavelEmailInscricao", "campo", "CampoInscricao deve ser informado");

            CampoInscricao = campoInscricao;
        }
        public virtual string CampoInscricao { get; protected set; }
    }

    public class VariavelEmailInscricaoInformado: AVariavelEmailInscricao
    {
        public VariavelEmailInscricaoInformado(string descricao, string variavel)
            : base(descricao, variavel)
        {
        }
    }
}
