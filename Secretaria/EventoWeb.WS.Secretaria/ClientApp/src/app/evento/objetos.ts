export enum SituacaoEvento {
  Aberto,
  EmAndamento,
  Concluido
}

export enum EnumPublicoEvangelizacao {
  Todos,
  TrabalhadoresOuParticipantesTrabalhadores
}

export enum EnumModeloDivisaoSalasEstudo {
  PorIdadeCidade,
  PorOrdemEscolhaInscricao
}

export class DTOEvento {
  public Nome: string;
  public PeriodoInscricao: Periodo;
  public PeriodoRealizacao: Periodo;
  public Logotipo: string;
  public TemDepartamentalizacao: Boolean;
  public TemOficinas: Boolean;
  public TemDormitorios: Boolean;
  public ConfiguracaoEvangelizacao: ConfiguracaoEvangelizacao;
  public ConfiguracaoSalaEstudo: ConfiguracaoSalaEstudo;
  public ConfiguracaoSarau: ConfiguracaoSarau;
}

export class Periodo {
  public DataInicial: Date;
  public DataFinal: Date;
}

export class ConfiguracaoEvangelizacao {
  public Publico: EnumPublicoEvangelizacao;
}

export class ConfiguracaoSarau {
  public TempoDuracaoMin: number;
}
export class ConfiguracaoSalaEstudo {
  public ModeloDivisao: EnumModeloDivisaoSalasEstudo;
}

export class DTOEventoCompleto extends DTOEvento {
  public Id: number;
  public DataRegistro: Date;
  public PodeAlterar: Boolean;
}

export class DTOEventoMinimo {
  public Id: number;
  public PeriodoInscricao: Periodo;
  public Nome: string;
  public Logotipo: string;
}

export class DTOId {
  public Id: number;
}
