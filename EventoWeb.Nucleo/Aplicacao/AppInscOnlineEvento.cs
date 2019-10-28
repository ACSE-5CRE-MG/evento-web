using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppInscOnlineEvento: AppBase
    {
        public AppInscOnlineEvento(IContexto contexto)
            : base(contexto) { }

        public DTOEventoListagem ObterPorIdDisponivelInscricaoOnline(int id)
        {
            DTOEventoListagem dtoEvento = null;
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(id);
                if (evento != null &&
                   evento.PeriodoInscricaoOnLine.DataInicial >= DateTime.Now &&
                   evento.PeriodoInscricaoOnLine.DataFinal <= DateTime.Now)
                    dtoEvento = Converter(evento);
            });

            return dtoEvento;
        }

        public IList<DTOEventoListagem> ListarEventosDisponiveisInscricaoOnline()
        {
            IList<DTOEventoListagem> dtoEventos = null;
            ExecutarSeguramente(() =>
            {
                var eventos = Contexto.RepositorioEventos.ObterTodosEventosEmPeriodoInscricaoOnline(DateTime.Now);
                dtoEventos = eventos.Select(x => Converter(x)).ToList();
            });

            return dtoEventos;
        }

        private DTOEventoListagem Converter(Evento evento)
        {
            return new DTOEventoListagem
            {
                Id = evento.Id,
                PeriodoInscricao = evento.PeriodoInscricaoOnLine,
                PeriodoRealizacao = evento.PeriodoRealizacaoEvento,
                IdadeMinima = evento.IdadeMinimaInscricaoAdulto,
                Logotipo = evento.Logotipo,
                Nome = evento.Nome
            };
        }
    }
}
