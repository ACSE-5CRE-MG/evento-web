export class DTOEventoListagem {
  public Id: number;
  public PeriodoInscricao: Periodo;
  public PeriodoRealizacao: Periodo;
  public Nome: string;
  public Logotipo: string;
}

export class Periodo {
  public DataInicial: Date;
  public DataFinal: Date;
}
