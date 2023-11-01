using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Persistencia.Relatorios;
using System.Collections.Generic;
using System.IO;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppGeracaoEtiquetaCracha
    {
        public Stream Gerar(IList<CrachaInscrito> crachas)
        {
            var gerador = new RelatorioEtiquetaCracha();
            return gerador.Gerar(crachas);
        }
    }
}
