using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AVariaveisEmailInscricao
    {
        public abstract IEnumerable<VariavelEmailInscricao> ListarTodas();
    }
}
