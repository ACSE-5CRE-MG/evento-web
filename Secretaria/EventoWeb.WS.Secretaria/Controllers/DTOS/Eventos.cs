using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventoWeb.WS.Secretaria.Controllers.DTOS
{
    public class DTOEvento
    {
        public string Nome { get; set; }
        public String DataInicioInscricao { get; set; }
        public String DataFimInscricao { get; set; }
        public String Logotipo { get; set; }

        public String EnderecoEmail { get; set; }
        public String UsuarioEmail { get; set; }
        public String SenhaEmail { get; set; }
        public String ServidorEmail { get; set; }
        public int? PortaServidor { get; set; }
        public String TipoSeguranca { get; set; }
        public String TituloEmailConfirmacaoInscricao { get; set; }
        public String MensagemEmailConfirmacaoInscricao { get; set; }
        public decimal ValorInscricao { get; set; }
        public string DataFimEvento { get; set; }
        public string DataInicioEvento { get; set; }
        public Boolean TemDepartamentalizacao { get; set; }
        public virtual Boolean TemSalasEstudo { get; set; }
        public virtual Boolean TemOficinas { get; set; }
        public virtual Boolean TemDormitorios { get; set; }
        public virtual Boolean TemEvangelizacao { get; set; }
        public virtual EnumPublicoEvangelizacao? PublicoEvangelizacao { get; set; }
        public virtual Boolean TemSarau { get; set; }
        public virtual int? TempoDuracaoSarauMin { get; set; }
        public virtual EnumModeloDivisaoSalasEstudo? ModeloDivisaoSalaEstudo { get; set; }
    }

    public class DTOEventoCompleto : DTOEvento
    {
        public int Id { get; set; }
        public SituacaoEvento Situacao { get; set; }
        public String DataRegistro { get; set; }
        public Boolean PodeAlterar { get; set; }
    }

    public class DTOEventoMinimo
    {
        public int Id { get; set; }
        public DateTime DataInicioInscricao { get; set; }
        public DateTime DataFimInscricao { get; set; }
        public SituacaoEvento Situacao { get; set; }
        public string Nome { get; set; }
        public String Logotipo { get; set; }
    }
}
