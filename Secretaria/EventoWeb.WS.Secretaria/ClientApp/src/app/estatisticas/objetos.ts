export class DTOEstatisticaTipoInscricao {

  public Criancas: number;

  public Participantes: number;

  public ParticipantesTrabalhadores: number;

  public Trabalhadores: number;

  public CriancasPresentes: number;

  public ParticipantesPresentes: number;

  public ParticipantesTrabalhadoresPresentes: number;

  public TrabalhadoresPresentes: number;
}

export class DTOEstatisticaSexo {

  public Homens: number;
  public Mulheres: number;
  public HomensPresentes: number;
  public MulheresPresentes: number;
}

export class DTOEstatisticaVegetariano {

  public Sao: number;

  public NaoSao: number;

  public SaoPresentes: number;

  public NaoSaoPresentes: number;
}

export class DTOEstatisticaAdocante {

  public Usam: number;
  public NaoUsam: number;
  public UsamPresentes: number;
  public NaoUsamPresentes: number;
}

export class DTOEstatisticaDiabeticos {

  public Sao: number;
  public NaoSao: number;
  public SaoPresentes: number;
  public NaoSaoPresentes: number;
}

export class DTOEstatisticaEvangelizacao {

  public NumeroMeninas: number;
  public NumeroMeninos: number;
  public NumeroCriancas0a3Anos: number;
  public NumeroCriancas4a6Anos: number;
  public NumeroCriancas7a9Anos: number;
  public NumeroCriancas10a12Anos: number;
  public NumeroMeninasPresentes: number;
  public NumeroMeninosPresentes: number;
  public NumeroCriancas0a3AnosPresentes: number;
  public NumeroCriancas4a6AnosPresentes: number;
  public NumeroCriancas7a9AnosPresentes: number;
  public NumeroCriancas10a12AnosPresentes: number;
}

export class DTOEstatisticaCidades {

  public Cidade: String;

  public NumeroInscricoes: number;
}

export class DTOEstatisticaGeral {

  public TotalInscricoes: number;

  public TotalInscricoesPresentes: number;

  public TotalInscricoesNaoDormem: number;

  public TotalInscricoesNaoDormemPresentes: number;

  public TiposInscricao: DTOEstatisticaTipoInscricao;

  public Sexo: DTOEstatisticaSexo;

  public Vegetarianos: DTOEstatisticaVegetariano;

  public UsamAdocante: DTOEstatisticaAdocante;

  public Diabeticos: DTOEstatisticaDiabeticos;

  public Evangelizacao: DTOEstatisticaEvangelizacao;

  public CarnesNaoCome: string[];

  public Medicamentos: string[];

  public Alergias: string[];

  public InscritosCidade: DTOEstatisticaCidades[];
}
