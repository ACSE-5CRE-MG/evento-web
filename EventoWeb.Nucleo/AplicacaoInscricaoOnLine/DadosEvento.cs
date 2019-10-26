using EventoWeb.Nucleo.Negocio.Entidades;

namespace EventoWeb.Nucleo.AplicacaoInscricaoOnLine
{
    public class DTOEventoListagem
    {
        public int Id { get; set; }
        public Periodo PeriodoInscricao { get; set; }
        public Periodo PeriodoRealizacao { get; set; }
        public string Nome { get; set; }
        public string Logotipo { get; set; }
        public int IdadeMinima { get; set; }
    }
}
