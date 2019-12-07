import { DTOEventoCompletoInscricao } from "../evento/objetos";
import { DTOSalaEstudo } from "../sala-estudo/objetos";
import { DTOOficina } from "../oficinas/objetos";
import { DTODepartamento } from "../departamentos/objetos";

export enum EnumSituacaoInscricao { Incompleta, Pendente, Aceita, Rejeitada }
export enum EnumSexo { Masculino, Feminino }
export enum EnumTipoInscricao { Participante, ParticipanteTrabalhador }

export enum EnumApresentacaoAtividades { ApenasParticipante, PodeEscolher }

export class DTOBasicoInscricao {
    IdInscricao: number;
    NomeInscrito: string;
    IdEvento: number;
    NomeEvento: string;
    Email: string;
    Cidade: string;
    UF: string;
    Situacao: EnumSituacaoInscricao;
    DataNascimento: Date;
    Tipo: string;
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

    Criancas: DTOCrianca[];

    Pagamento: DTOPagamento;
}

export class DTOInscricaoCompleta extends DTOInscricaoAtualizacao {
    Id: number;
    Evento: DTOEventoCompletoInscricao;
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
