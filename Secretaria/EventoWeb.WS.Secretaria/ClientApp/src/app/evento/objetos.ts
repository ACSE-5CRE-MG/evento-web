import { extend } from "webdriver-js-extender";
import { DTOOficina } from "../oficinas/objetos";
import { DTOSalaEstudo } from "../sala-estudo/objetos";
import { DTODepartamento } from "../departamentos/objetos";

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

export enum EnumModeloDivisaoOficinas {
  PorOrdemEscolhaInscricao, PorIdadeCidade
}

export class DTOEvento {
  public Nome: string;
  public PeriodoInscricao: Periodo;
  public PeriodoRealizacao: Periodo;
  public Logotipo: string;
  public TemDepartamentalizacao: Boolean;
  public TemDormitorios: Boolean;
  public ConfiguracaoEvangelizacao: EnumPublicoEvangelizacao;
  public ConfiguracaoSalaEstudo: EnumModeloDivisaoSalasEstudo;
  public ConfiguracaoTempoSarauMin: number;
  public ConfiguracaoOficinas: EnumModeloDivisaoOficinas;
  public PermiteEscolhaDormirEvento: Boolean;
}

export class Periodo {
  public DataInicial: Date;
  public DataFinal: Date;
}

export class ConfiguracaoSarau {
  public TempoDuracaoMin: number;
}

export class DTOEventoCompleto extends DTOEvento {
  public Id: number;
  public DataRegistro: Date;
  public PodeAlterar: Boolean;
  IdadeMinima: number;
  ValorInscricaoAdulto: number;
  ValorInscricaoCrianca: number;

}

export class DTOEventoCompletoInscricao extends DTOEventoCompleto {
  Oficinas: DTOOficina[];
  SalasEstudo: DTOSalaEstudo[];
  Departamentos: DTODepartamento[];
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

