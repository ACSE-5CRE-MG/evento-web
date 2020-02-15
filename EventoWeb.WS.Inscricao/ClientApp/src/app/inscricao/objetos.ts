import { DTOOficina, DTOEventoCompleto, DTOSalaEstudo, DTODepartamento } from '../principal/objetos';

export enum EnumSexo { Masculino, Feminino }
export enum EnumTipoInscricao { Participante, ParticipanteTrabalhador, Trabalhador }
export enum EnumApresentacaoAtividades {
    ApenasParticipante, PodeEscolher
}
export enum EnumSituacaoInscricao { Incompleta, Pendente, Aceita, Rejeitada }

export class DTODadosConfirmacao {
    public IdInscricao: number;
    public EnderecoEmail: string;
}

export class DTOBasicoInscricao {
    public IdInscricao: number;
    public NomeInscrito: string;
    public IdEvento: number;
    public NomeEvento: string;
    public Email: string;
}

export class DTOAcessoInscricao {
    public IdInscricao: number;
    public Autorizacao: string;
}

export enum EnumResultadoEnvio { InscricaoNaoEncontrada, EventoEncerradoInscricao, InscricaoOK }

export class DTOEnvioCodigoAcessoInscricao {
  public Resultado: EnumResultadoEnvio;
  public IdInscricao: number;
}

export class DTOInscricaoDadosPessoais {
    Nome: string;
    DataNascimento: Date;
    Email: string;
    Sexo: EnumSexo;
    Cidade: string;
    Uf: string;
    EhVegetariano: boolean;
    UsaAdocanteDiariamente: boolean;
    EhDiabetico: boolean;
    CarnesNaoCome: string;
    AlimentosAlergia: string;
    MedicamentosUsa: string;
    Celular: string;
    TelefoneFixo: string;
}

export class DTOInscricaoAtualizacao {
    DadosPessoais: DTOInscricaoDadosPessoais;
    TipoInscricao: EnumTipoInscricao;
    NomeCracha: string;
    CentroEspirita: string;
    TempoEspirita: string;
    NomeResponsavelCentro: string;
    TelefoneResponsavelCentro: string;
    NomeResponsavelLegal: string;
    TelefoneResponsavelLegal: string;
    PrimeiroEncontro: boolean;
    Observacoes: string;

    Oficina: DTOInscricaoOficina;
    SalasEstudo: DTOInscricaoSalaEstudo;
    Departamento: DTOInscricaoDepartamento;
    Sarais: DTOSarau[];

    Pagamento: DTOPagamento;
}

export class DTOInscricaoCompleta extends DTOInscricaoAtualizacao {
    Id: number;
    Evento: DTOEventoCompleto;
    Situacao: EnumSituacaoInscricao;
}

export class DTOInscricaoOficina {
    Coordenador: DTOOficina;
    EscolhidasParticipante: DTOOficina[];
}

export class DTOInscricaoSalaEstudo {
    Coordenador: DTOSalaEstudo;
    EscolhidasParticipante: DTOSalaEstudo[];
}

export class DTOInscricaoDepartamento {
    Coordenador: DTODepartamento;
    Participante: DTODepartamento;
}

export class DTOSarau {
    Id: number;
    Tipo: string;
    DuracaoMin: number;
    Participantes: DTOInscricaoSimplificada[];
}

export class DTOInscricaoSimplificada {
    Id: number;
    IdEvento: number;
    Nome: string;
}

export class DTOCrianca {
    Id: number;
    Nome: string;
    DataNascimento: Date;
    Sexo: EnumSexo;
    EhVegetariano: boolean;
    UsaAdocanteDiariamente: boolean;
    EhDiabetico: boolean;
    CarnesNaoCome: string;
    AlimentosAlergia: string;
    MedicamentosUsa: string;
    Responsaveis: DTOInscricaoSimplificada[];
    Cidade: string;
    Uf: string;
    Email: string;
    NomeCracha: string;
    PrimeiroEncontro: boolean;
    Sarais: DTOSarau[];
}

export enum EnumPagamento { Comprovante, ComprovanteOutraInscricao, Outro }

export class DTOPagamento {

    Forma: EnumPagamento;
    ComprovantesBase64: string[];
    Observacao: string;
}
