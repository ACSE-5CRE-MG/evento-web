using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface AAfracs : IPersistencia<Afrac>
    {
        IList<Afrac> ListarTodasAfracsPorEvento(int idEvento);
        Afrac ObterAfracPorIdEventoEAfrac(int idEvento, int idAfrac);
        IList<Afrac> ListarTodasAfracsComParticipantesPorEvento(Evento evento);
        IList<InscricaoParticipante> ListarParticipantesSemAfracNoEvento(Evento evento);
        bool InscritoEhResponsavelPorAfrac(Evento evento, InscricaoParticipante inscParticipante);

        bool HaAfracsSemResponsavelDefinidoDoEvento(Evento evento);

        bool EhParticipanteDeAfracNoEvento(Evento evento, InscricaoParticipante participante);
        bool HaParticipatesOuResponsaveisEmOutraAfrac(Afrac afrac);

        Afrac BuscarAfracDoInscrito(int idEvento, int idInscricao);
        int ContarTotalAfracs(Evento mEvento);
    }
}
