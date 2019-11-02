using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface AOficinas : IPersistencia<Oficina>
    {
        IList<Oficina> ListarTodasPorEvento(int idEvento);
        Oficina ObterPorId(int id);
        IList<Oficina> ListarTodasComParticipantesPorEvento(Evento evento);
        IList<InscricaoParticipante> ListarParticipantesSemOficinaNoEvento(Evento evento);
        bool InscritoEhResponsavelPorOficina(Evento evento, InscricaoParticipante inscParticipante);

        bool HaAOficinasSemResponsavelDefinidoDoEvento(Evento evento);

        bool EhParticipanteDeOficinaNoEvento(Evento evento, InscricaoParticipante participante);
        bool HaParticipatesOuResponsaveisEmOutraOficina(Oficina afrac);

        Oficina BuscarOficinaDoInscrito(int idEvento, int idInscricao);
        int ContarTotalOficinas(Evento mEvento);
    }
}
