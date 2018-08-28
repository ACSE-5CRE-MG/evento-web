using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AQuartos: ARepositorio<Quarto>
    {
        public AQuartos(IPersistencia<Quarto> persistencia) : base(persistencia) { }
        public abstract IList<Quarto> ListarTodosQuartosPorEvento(int idEvento);
        public abstract Quarto ObterQuartoPorIdEventoEQuarto(int idEvento, int idQuarto);
        public abstract Boolean HaOutroQuartoComCapacidadeInfinita(Quarto quarto);

        public abstract IList<Quarto> ListarTodosQuartosPorEventoComParticipantes(int idEvento);

        public abstract Quarto BuscarQuartoDoInscrito(int idEvento, int idInscricao);
    }
}
