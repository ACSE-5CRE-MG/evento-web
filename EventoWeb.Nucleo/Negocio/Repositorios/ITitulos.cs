using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface ITitulos : IPersistencia<Titulo>
    {
        IList<Titulo> ListarTodos(int idEvento, DateTime dataInicial, DateTime dataFinal, String nomeCliente,
            TipoSituacaoTitulo[] situacoes, TipoTransacao tipo);
        ParcelaTitulo ObterParcelaPorId(int id);
        bool HaTituloVinculadoTransacao(int idTransacao);

        Titulo ObterPorId(int id);
    }
}
