using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface ICategorias : IPersistencia<Categoria>
    {
        IList<Categoria> ListarTodos(int idEvento, TipoTransacao tipoTransacao);

        Categoria ObterPorId(int id);
    }
}
