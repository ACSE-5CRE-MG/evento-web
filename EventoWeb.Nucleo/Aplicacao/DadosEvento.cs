using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class DTOEvento
    {
        public string Nome { get; set; }
        public Periodo PeriodoInscricao { get; set; }
        public Periodo PeriodoRealizacao { get; set; }
        public String Logotipo { get; set; }
        public Boolean TemDepartamentalizacao { get; set; }
        public Boolean TemOficinas { get; set; }
        public Boolean TemDormitorios { get; set; }
        public ConfiguracaoEvangelizacao ConfiguracaoEvangelizacao { get; set; }
        public ConfiguracaoSalaEstudo ConfiguracaoSalaEstudo { get; set; }
        public ConfiguracaoSarau ConfiguracaoSarau { get; set; }
    }

    public class DTOEventoCompleto : DTOEvento
    {
        public int Id { get; set; }
        public DateTime DataRegistro { get; set; }
        public Boolean PodeAlterar { get; set; }
    }

    public class DTOEventoMinimo
    {
        public int Id { get; set; }
        public Periodo PeriodoInscricao { get; set; }
        public string Nome { get; set; }
        public String Logotipo { get; set; }
    }
}
