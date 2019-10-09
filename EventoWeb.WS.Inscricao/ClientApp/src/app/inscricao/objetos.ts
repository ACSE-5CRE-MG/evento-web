import { DTOOficina, DTOEventoCompleto, DTOSalaEstudo } from '../principal/objetos';

export enum EnumSexo { Feminino, Masculino }
export enum EnumTipoInscricao { Participante, ParticipanteTrabalhador }
export enum EnumApresentacaoAtividades {
  ApenasParticipante, PodeEscolher
}

export class DTODadosCriarInscricao {
  Nome: string;
  DataNascimento: Date;
  Email: string;
  Sexo: EnumSexo;
  TipoInscricao: EnumTipoInscricao;
  Cidade: string;
  UF: string;
}

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
  TipoInscricao: EnumTipoInscricao;
  Cidade: string;
  Uf: string;
  EhVegetariano: boolean;
  UsaAdocanteDiariamente: boolean;
  EhDiabetico: boolean;
  CarnesNaoCome: string;
  AlimentosAlergia: string;
  MedicamentosUsa: string;
  PrimeiroEncontro: boolean;
}

export class DTOInscricaoCompleta {
  Id: number;
  Evento: DTOEventoCompleto;
  DadosPessoais: DTOInscricaoDadosPessoais;
  CentroEspirita: string;
  TempoEspirita: string;
  NomeResponsavelCentro: string;
  TelefoneResponsavelCentro: string;
  NomeResponsavelLegal: string;
  TelefoneResponsavelLegal: string;

  Oficina: DTOInscricaoOficina;
  SalasEstudo: DTOInscricaoSalaEstudo;  
}

export class DTOInscricaoOficina {
  Coordenador: DTOOficina;
  OficinasEscolhidasParticipante: DTOOficina[];
}

export class DTOInscricaoSalaEstudo {
  Coordenador: DTOSalaEstudo;
  SalasEscolhidasParticipante: DTOSalaEstudo[];
}
