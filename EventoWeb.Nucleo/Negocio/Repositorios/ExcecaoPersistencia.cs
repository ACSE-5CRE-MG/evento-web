using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class ExcecaoPersistencia : Exception
    {
        public ExcecaoPersistencia(string mensagem): base(mensagem)
        {
        }
    }
}
