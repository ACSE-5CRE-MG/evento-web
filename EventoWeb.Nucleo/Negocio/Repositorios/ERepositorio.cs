using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class ERepositorio: Exception
    {
        public ERepositorio(String mensagem) : base(mensagem) { }
    }
}
