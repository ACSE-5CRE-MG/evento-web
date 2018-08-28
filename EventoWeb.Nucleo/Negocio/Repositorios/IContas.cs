using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface IContas: IPersistencia<Conta>
    {
        IList<Conta> ListarTodos(int idEvento);

        Conta ObterPorId(int id);
    }
}
