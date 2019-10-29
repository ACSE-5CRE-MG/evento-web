using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface IServicoGeradorCodigoSeguro
    {
        string GerarCodigo5Caracteres();
    }
}
