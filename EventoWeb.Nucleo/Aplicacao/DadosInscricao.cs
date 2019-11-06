using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public enum EnumApresentacaoAtividades { ApenasParticipante, PodeEscolher }
    public enum EnumResultadoEnvio { InscricaoNaoEncontrada, EventoEncerradoInscricao, InscricaoOK, IdentificacaoInvalida }

    public class DTODadosCriarInscricao
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public SexoPessoa Sexo { get; set; }
        public EnumTipoParticipante TipoInscricao { get; set; }
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
        public EnumSituacaoInscricao Situacao { get; set; }
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

    public class DTOInscricaoDadosPessoais
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public SexoPessoa Sexo { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public Boolean EhVegetariano { get; set; }
        public Boolean UsaAdocanteDiariamente { get; set; }
        public Boolean EhDiabetico { get; set; }
        public string CarnesNaoCome { get; set; }
        public string AlimentosAlergia { get; set; }
        public string MedicamentosUsa { get; set; }
        public string Celular { get; internal set; }
        public string TelefoneFixo { get; internal set; }
    }

    public class DTOInscricaoAtualizacao
    {
        public DTOInscricaoDadosPessoais DadosPessoais { get; set; }
        public EnumTipoParticipante TipoInscricao { get; set; }
        public string NomeCracha { get; set; }
        public string CentroEspirita { get; set; }
        public string TempoEspirita { get; set; }
        public string NomeResponsavelCentro { get; set; }
        public string TelefoneResponsavelCentro { get; set; }
        public string NomeResponsavelLegal { get; set; }
        public string TelefoneResponsavelLegal { get; set; }
        public bool PrimeiroEncontro { get; set; }
        public string Observacoes { get; set; }
        public DTOInscricaoOficina Oficina { get; set; }
        public DTOInscricaoSalaEstudo SalasEstudo { get; set; }
        public DTOInscricaoDepartamento Departamento { get; set; }
        public IList<DTOSarau> Sarais { get; set; }
        public IList<DTOCrianca> Criancas { get; set; }
        public DTOPagamento Pagamento { get; set; }
    }

    public class DTOInscricaoCompleta: DTOInscricaoAtualizacao
    {
        public int Id { get; set; }
        public DTOEventoCompleto Evento { get; set; }
        public EnumSituacaoInscricao Situacao { get; set; }
    }

    public class DTOInscricaoOficina
    {
        public DTOOficina Coordenador { get; set; }
        public IList<DTOOficina> EscolhidasParticipante { get; set; }
    }

    public class DTOInscricaoSalaEstudo
    {
        public DTOSalaEstudo Coordenador { get; set; }
        public IList<DTOSalaEstudo> EscolhidasParticipante { get; set; }
    }

    public class DTOInscricaoDepartamento
    {
        public DTODepartamento Coordenador { get; set; }
        public DTODepartamento Participante { get; set; }
    }

    public class DTOSarau
    {
        public int? Id { get; set; }
        public string Tipo { get; set; }
        public int DuracaoMin { get; set; }
        public IList<DTOInscricaoSimplificada> Participantes { get; set; }
    }

    public class DTOInscricaoSimplificada
    {
        public int Id { get; set; }
        public int IdEvento { get; set; }
        public string Nome { get; set; }
    }

    public class DTOCrianca
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public SexoPessoa Sexo { get; set; }
        public bool EhVegetariano { get; set; }
        public bool UsaAdocanteDiariamente { get; set; }
        public bool EhDiabetico { get; set; }
        public string CarnesNaoCome { get; set; }
        public string AlimentosAlergia { get; set; }
        public string MedicamentosUsa { get; set; }
        public IList<DTOInscricaoSimplificada> Responsaveis { get; set; }
        public string Cidade { get; internal set; }
        public string Uf { get; internal set; }
        public string Email { get; internal set; }
        public string NomeCracha { get; internal set; }
        public bool PrimeiroEncontro { get; internal set; }
    }

    public class DTOPagamento
    {
        public EnumPagamento Forma { get; set; }
        public IList<string> ComprovantesBase64 { get; set; }
        public string Observacao { get; set; }
    }
}
