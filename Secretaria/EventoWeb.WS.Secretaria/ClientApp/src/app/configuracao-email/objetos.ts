export enum TipoSegurancaEmail { SSL, Nenhuma }

export class DTOConfiguracaoEmail {
  public EnderecoEmail: String;

  public UsuarioEmail: String;

  public SenhaEmail: String;

  public ServidorEmail: String;

  public PortaServidor: number;

  public TipoSeguranca: TipoSegurancaEmail;
}
