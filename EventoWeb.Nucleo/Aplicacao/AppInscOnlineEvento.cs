using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppInscOnlineEvento: AppBase
    {
        public AppInscOnlineEvento(IContexto contexto)
            : base(contexto) { }

        public DTOEventoMinimo ObterPorIdDisponivelInscricaoOnline(int id)
        {
            DTOEventoMinimo dtoEvento = null;
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(id);
                if (evento != null &&
                   evento.PeriodoInscricaoOnLine.DataInicial <= DateTime.Now &&
                   evento.PeriodoInscricaoOnLine.DataFinal >= DateTime.Now)
                    dtoEvento = evento.ConverterMinimo();
            });

            return dtoEvento;
        }

        public IList<DTOEventoMinimo> ListarEventosDisponiveisInscricaoOnline()
        {
            IList<DTOEventoMinimo> dtoEventos = null;
            ExecutarSeguramente(() =>
            {
                var eventos = Contexto.RepositorioEventos.ObterTodosEventosEmPeriodoInscricaoOnline(DateTime.Now);
                dtoEventos = eventos.Select(x => x.ConverterMinimo()).ToList();
            });

            return dtoEventos;
        }

        public DTOEventoCompleto ObterPorIdCompleto(int id)
        {
            DTOEventoCompleto dtoEvento = null;
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(id);
                dtoEvento = evento?.ConverterApenasEvento();
            });

            return dtoEvento;
        }
    }
}
