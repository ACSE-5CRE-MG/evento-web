using System;
using System.Collections.Generic;
using System.Text;
using EventoWeb.Nucleo.Negocio.Entidades;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class DTOSalaEstudo : DTOId
    {
        public string Nome { get; set; }
        public bool DeveSerParNumeroTotalParticipantes { get; set; }
        public int? IdadeMinima { get; set; }
        public int? IdadeMaxima { get; set; }
    }
}
