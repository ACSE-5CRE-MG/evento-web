using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class CodigoAcessoInscricao: Entidade
    {
        public CodigoAcessoInscricao(string codigo, Inscricao inscricao, DateTime dataHoraValidade)
        {
            Inscricao = inscricao ?? throw new ArgumentNullException("inscricao");
            Codigo = codigo;
            DataHoraValidade = dataHoraValidade;

        }

        public virtual Inscricao Inscricao { get; protected set; }
        public virtual string Codigo { get; protected set; }
        public virtual DateTime DataHoraValidade { get; protected set; }
    }
}
