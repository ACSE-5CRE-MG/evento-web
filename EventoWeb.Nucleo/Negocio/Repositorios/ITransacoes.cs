using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface ITransacoes : IPersistencia<Transacao>
    {
        decimal ObterTotalTransacoesPorData(int idConta, DateTime data);
        IList<Transacao> ListarTodos(int idEvento, DateTime dataInicial, DateTime dataFinal, int idConta, 
            bool listarPagamentos, bool listarRecebimentos);
        DateTime? ObterDataUltimaTransacaoDaConta(int idConta);
        Transacao ObterPorId(int id);
    }
}
