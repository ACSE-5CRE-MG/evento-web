using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{ 
    public interface ASalasEstudo: IPersistencia<SalaEstudo>
    {
        SalaEstudo ObterPorId(int idEvento, int id);
        IList<SalaEstudo> ListarTodasPorEvento(int idEvento);
        IList<SalaEstudo> ListarTodasSalasEstudoComParticipantesDoEvento(Evento evento);
        Boolean EhCoordenadorDeSalaNoEvento(Evento evento, InscricaoParticipante participante);
        bool HaSalasSemCoordenadorDefinidoDoEvento(Evento evento);
        bool EhParticipanteDeSalaNoEvento(Evento evento, InscricaoParticipante participante);
        bool HaSalaComFaixaEtariaDefinida(SalaEstudo sala);
        IList<InscricaoParticipante> ListarParticipantesSemSalaEstudoNoEvento(Evento evento);

        bool HaParticipatesEmOutraSala(SalaEstudo sala);
        int ContarTotalSalas(Evento evento);
        SalaEstudo BuscarSalaDoInscrito(int idEvento, int idInscricao);
    }
}
