export enum EnumSexo { Feminino, Masculino }

export class DTODadosCriarInscricao {
  Nome: string;
  DataNascimento: Date;
  Email: string;
  Sexo: EnumSexo;
}

export class DTODadosConfirmacao {
  public IdInscricao: number;
  public EnderecoEmail: string;
}

export class DTOAcessoInscricao {
  public IdInscricao: number;
  public NomeInscrito: string;
  public IdEvento: number;
  public NomeEvento: string;
  public Email: string;
}
