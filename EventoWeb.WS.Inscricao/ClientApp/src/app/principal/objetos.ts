export class DTOEventoListagem {
    public Id: number;
    public PeriodoInscricao: Periodo;
    public PeriodoRealizacao: Periodo;
    public Nome: string;
    public Logotipo: string;
    public IdadeMinima: number;
    PermiteInscricaoInfantil: boolean;
}

export class Periodo {
    public DataInicial: Date;
    public DataFinal: Date;
}

export class DTOEventoCompleto extends DTOEventoListagem {
    Oficinas: DTOOficina[];
    SalasEstudo: DTOSalaEstudo[];
    Departamentos: DTODepartamento[];
    ConfiguracaoOficinas: EnumModeloDivisaoOficinas;
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

export enum EnumModeloDivisaoOficinas { PorOrdemEscolhaInscricao, PorIdadeCidade }

export class DTOContratoInscricao
{
  Id: number;
  Regulamento: string;
  InstrucoesPagamento: string;
  PassoAPassoInscricao: string;
}

