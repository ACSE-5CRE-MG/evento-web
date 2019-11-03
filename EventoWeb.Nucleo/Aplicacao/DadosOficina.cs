﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class DTOOficina : DTOId
    {
        public string Nome { get; set; }
        public bool DeveSerParNumeroTotalParticipantes { get; set; }
        public int? NumeroTotalParticipantes { get; set; }
    }
}