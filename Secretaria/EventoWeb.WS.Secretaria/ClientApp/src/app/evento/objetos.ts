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

export class DTOEvento {
    public Nome: string;
    public PeriodoInscricao: Periodo;
    public PeriodoRealizacao: Periodo;
    public Logotipo: string;
    public TemDepartamentalizacao: Boolean;
    public TemOficinas: Boolean;
    public TemDormitorios: Boolean;
    public ConfiguracaoEvangelizacao: EnumPublicoEvangelizacao;
    public ConfiguracaoSalaEstudo: EnumModeloDivisaoSalasEstudo;
    public ConfiguracaoTempoSarauMin: number;
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
}

export class DTOEventoCompletoInscricao extends DTOEventoCompleto {
    IdadeMinima: number;
    Oficinas: DTOOficina[];
    SalasEstudo: DTOSalaEstudo[];
    Departamentos: DTODepartamento[];
    ValorInscricaoAdulto: number;
    ValorInscricaoCrianca: number;
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

