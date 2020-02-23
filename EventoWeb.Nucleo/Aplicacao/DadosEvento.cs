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
        public EnumPublicoEvangelizacao? ConfiguracaoEvangelizacao { get; set; }
        public EnumModeloDivisaoSalasEstudo? ConfiguracaoSalaEstudo { get; set; }
        public int? ConfiguracaoTempoSarauMin { get; set; }
        public int IdadeMinima { get; set; }
    }

    public class DTOEventoCompleto : DTOEvento
    {
        public int Id { get; set; }
        public DateTime DataRegistro { get; set; }
        public decimal ValorInscricaoAdulto { get; set; }
        public decimal ValorInscricaoCrianca { get; set; }
    }

    public class DTOEventoCompletoInscricao: DTOEventoCompleto
    {
        public Boolean PodeAlterar { get; set; }
        public IEnumerable<DTOSalaEstudo> SalasEstudo { get; set; }
        public IEnumerable<DTODepartamento> Departamentos { get; set; }
        public IEnumerable<DTOOficina> Oficinas { get; set; }
    }

    public class DTOEventoMinimo
    {
        public int Id { get; set; }
        public Periodo PeriodoInscricao { get; set; }
        public Periodo PeriodoRealizacao { get; set; }
        public string Nome { get; set; }
        public String Logotipo { get; set; }
        public int IdadeMinima { get; set; }
        public bool PermiteInscricaoInfantil { get; set; }
    }
}
