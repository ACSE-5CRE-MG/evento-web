using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface IExtratos
    {
        ExtratoConta Buscar(Conta conta, DateTime dataInicial, DateTime dataFinal);
    }
}
