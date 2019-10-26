using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.AplicacaoInscricaoOnLine
{
    public class AppEvento: AppBase
    {
        public AppEvento(IContexto contexto)
            : base(contexto) { }

        public DTOEventoListagem ObterPorId(int id)
        {
            DTOEventoListagem dtoEvento = null;
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(id);
                if (evento != null && 
                   evento.PeriodoInscricaoOnLine.DataInicial >= DateTime.Now && 
                   evento.PeriodoInscricaoOnLine.DataFinal <= DateTime.Now)
                    dtoEvento = new DTOEventoListagem
                    {
                        Id = evento.Id,
                        PeriodoInscricao = evento.PeriodoInscricaoOnLine,
                        PeriodoRealizacao = evento.PeriodoRealizacaoEvento,
                        IdadeMinima = evento.IdadeMinimaInscricaoAdulto,
                        Logotipo = evento.Logotipo,
                        Nome = evento.Nome
                    };
            });

            return dtoEvento;
        }

        public IList<DTOEventoListagem> ListarEventosDisponiveis()
        {
            IList<DTOEventoListagem> dtoEventos = null;
            ExecutarSeguramente(() =>
            {
                var eventos = Contexto.RepositorioEventos.ObterTodosEventosEmPeriodoInscricaoOnline(DateTime.Now);
                dtoEventos = eventos.Select(x => new DTOEventoListagem()
                {
                    Id = x.Id,
                    PeriodoInscricao = x.PeriodoInscricaoOnLine,
                    PeriodoRealizacao = x.PeriodoRealizacaoEvento,
                    IdadeMinima = x.IdadeMinimaInscricaoAdulto,
                    Logotipo = x.Logotipo,
                    Nome = x.Nome
                }).ToList();
            });

            return dtoEventos;
        }
    }
}
