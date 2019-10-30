using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioVariaveisEmailIncricao : AVariaveisEmailInscricao
    {
        public override IEnumerable<VariavelEmailInscricao> ListarTodas()
        {
            return new List<VariavelEmailInscricao>()
            {
                new VariavelEmailInscricao("Nome","#[NOME]","Pessoa.Nome"),
                new VariavelEmailInscricao("Evento","#[EVENTO]","Evento.nome"),
            };
        }
    }
}
