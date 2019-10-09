export class DTOEventoListagem {
  public Id: number;
  public PeriodoInscricao: Periodo;
  public PeriodoRealizacao: Periodo;
  public Nome: string;
  public Logotipo: string;
  public IdadeMinima: number;
}

export class Periodo {
  public DataInicial: Date;
  public DataFinal: Date;
}

export class DTOEventoCompleto extends DTOEventoListagem {
  Oficinas: DTOOficina[];
  SalasEstudo: DTOSalaEstudo[];
  TemOficinas: boolean;
  TemDepartamentos: boolean;
  TemSarau: boolean;
  CnfSalaEstudo: EnumModeloDivisaoSalasEstudo; // Pode ser nulo
  CnfEvangelizacao: EnumPublicoEvangelizacao; // Pode ser nulo
}

export class DTOOficina {
  public Id: number;
  public Descricao: string;
}

export class DTOSalaEstudo {
  public Id: number;
  public Descricao: string;
}

export enum EnumModeloDivisaoSalasEstudo { PorIdadeCidade, PorOrdemEscolhaInscricao }

export enum EnumPublicoEvangelizacao { Todos, TrabalhadoresOuParticipantesTrabalhadores }

