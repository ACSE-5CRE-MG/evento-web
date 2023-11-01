using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Persistencia.Relatorios;
using System.Collections.Generic;
using System.IO;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppGeracaoEtiquetaCaderno
    {
        public Stream Gerar(IList<CrachaInscrito> crachas)
        {
            var gerador = new RelatorioEtiquetaCaderno();
            return gerador.Gerar(crachas);
        }
    }
}
