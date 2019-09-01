export class DTODadosCriarInscricao {
  Nome: string;
  DataNascimento: Date;
  Email: string; 
}

export class DTODadosConfirmacao {
  public IdInscricao: number;
  public EnderecoEmail: string;
}
