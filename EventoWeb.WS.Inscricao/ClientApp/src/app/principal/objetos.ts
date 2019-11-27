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
    Departamentos: DTODepartamento[];
    TemOficinas: boolean;
    TemDepartamentalizacao: boolean;
    ConfiguracaoTempoSarauMin: number; // Pode ser nulo
    ConfiguracaoSalaEstudo: EnumModeloDivisaoSalasEstudo; // Pode ser nulo
    ConfiguracaoEvangelizacao: EnumPublicoEvangelizacao; // Pode ser nulo
    ValorInscricaoAdulto: number;
    ValorInscricaoCrianca: number;
}

export class DTOOficina {
    public Id: number;
    public Nome: string;
}

export class DTOSalaEstudo {
    public Id: number;
    public Nome: string;
}

export class DTODepartamento {
    public Id: number;
    public Nome: string;
}

export enum EnumModeloDivisaoSalasEstudo { PorIdadeCidade, PorOrdemEscolhaInscricao }

export enum EnumPublicoEvangelizacao { Todos, TrabalhadoresOuParticipantesTrabalhadores }

