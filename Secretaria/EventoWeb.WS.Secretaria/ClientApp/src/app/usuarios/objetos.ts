export class DTOUsuario {
  Login: string;
  Nome: string;
  EhAdministrador: boolean;
}

export class DTOUsuarioInclusao extends DTOUsuario
{
  Senha: string;
  RepeticaoSenha: string;
}

export class DTOAlteracaoSenhaWS {
  NovaSenha: string;
  NovaSenhaRepetida: string;
}

export class DTOAlteracaoSenhaComumWS extends DTOAlteracaoSenhaWS
{
  SenhaAtual: string;
}
