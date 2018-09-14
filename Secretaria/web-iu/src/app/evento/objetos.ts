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
  public DataInicioInscricao: String;
  public DataFimInscricao: String;
  public Logotipo: String;
  public EnderecoEmail: String;
  public UsuarioEmail: String;
  public SenhaEmail: String;
  public ServidorEmail: String;
  public PortaServidor: number;
  public TipoSeguranca: String;
  public TituloEmailConfirmacaoInscricao: String;
  public MensagemEmailConfirmacaoInscricao: String;
  public ValorInscricao: number;
  public DataFimEvento: string;
  public DataInicioEvento: string;
  public TemDepartamentalizacao: Boolean;
  public TemSalasEstudo: Boolean;
  public TemOficinas: Boolean;
  public TemDormitorios: Boolean;
  public TemEvangelizacao: Boolean;
  public PublicoEvangelizacao: EnumPublicoEvangelizacao;
  public TemSarau: Boolean;
  public TempoDuracaoSarauMin: number;
  public ModeloDivisaoSalaEstudo: EnumModeloDivisaoSalasEstudo;
}

export class DTOEventoCompleto extends DTOEvento {
  public Id: number;
  public Situacao: SituacaoEvento;
  public DataRegistro: String;
  public PodeAlterar: Boolean;
}

export class DTOEventoMinimo {
  public Id: number;
  public DataInicioInscricao: Date;
  public DataFimInscricao: Date;
  public Situacao: SituacaoEvento;
  public Nome: string;
  public Logotipo: String;
}
