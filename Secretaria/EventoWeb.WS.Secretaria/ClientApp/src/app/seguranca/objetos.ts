import { DTOUsuario } from "../usuarios/objetos";

export class DTWDadosAutenticacao {
  Login: string;
  Senha: string;
}

export class DTWAutenticacao {
  Usuario: DTOUsuario;
  TokenAutenticacao: string;
  Validade: Date;
}
