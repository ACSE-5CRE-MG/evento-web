using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface AApresentacoesSarau : IPersistencia<ApresentacaoSarau>
    {
        IList<ApresentacaoSarau> ListarTodas(int idEvento);
        int ObterTempoTotalApresentacoes(Evento evento, ApresentacaoSarau apresentacaoNaoConsiderar = null);
        ApresentacaoSarau ObterPorId(int idEvento, int id);
        IList<ApresentacaoSarau> ListarPorInscricao(int idInscricao);
    }
}
