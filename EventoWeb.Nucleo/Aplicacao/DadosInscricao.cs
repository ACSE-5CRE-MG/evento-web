using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public enum EnumSexo { Feminino, Masculino }
    public enum EnumTipoInscricao { Participante, ParticipanteTrabalhador }
    public enum EnumApresentacaoAtividades { ApenasParticipante, PodeEscolher }
    public enum EnumResultadoEnvio { InscricaoNaoEncontrada, EventoEncerradoInscricao, InscricaoOK, IdentificacaoInvalida }

    public class DTODadosCriarInscricao
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public EnumSexo Sexo { get; set; }
        public EnumTipoInscricao TipoInscricao { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
    }

    public class DTODadosConfirmacao
    {
        public int IdInscricao { get; set; }
        public string EnderecoEmail { get; set; }
    }

    public class DTOBasicoInscricao
    {
        public int IdInscricao { get; set; }
        public string NomeInscrito { get; set; }
        public int IdEvento { get; set; }
        public string NomeEvento { get; set; }
        public string Email { get; set; }
    }

    public class DTOAcessoInscricao
    {
        public int IdInscricao { get; set; }
        public string Autorizacao { get; set; }
    }    

    public class DTOEnvioCodigoAcessoInscricao
    {
        public EnumResultadoEnvio Resultado { get; set; }
        public int? IdInscricao { get; set; }
    }
}
