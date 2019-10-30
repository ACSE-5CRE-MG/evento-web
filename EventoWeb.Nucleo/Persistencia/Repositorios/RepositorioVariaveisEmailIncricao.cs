using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioVariaveisEmailIncricao : AVariaveisEmailInscricao
    {
        public override IEnumerable<AVariavelEmailInscricao> ListarTodas()
        {
            return new List<AVariavelEmailInscricao>()
            {
                new VariavelEmailInscricao("Nome","#[NOME]","Pessoa.Nome"),
                new VariavelEmailInscricao("Evento","#[EVENTO]","Evento.nome"),
                new VariavelEmailInscricaoInformado("Codigo","#[CODIGO]")
            };
        }
    }
}
