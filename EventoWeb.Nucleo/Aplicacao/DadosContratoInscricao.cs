using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class DTOContratoInscricao: DTOId
    {
        public virtual string Regulamento { get; set; }
        public virtual string InstrucoesPagamento { get; set; }
        public virtual string PassoAPassoInscricao { get; set; }
    }
}
