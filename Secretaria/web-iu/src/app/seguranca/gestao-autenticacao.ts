import { Injectable } from '@angular/core';
import { CookieService, CookieOptions } from 'ngx-cookie';

@Injectable()
export class GestaoAutenticacao {

  public NOME_STORANGE: string = "eventoweb-autenticacao";
  public mAutenticacao: Autenticacao = null;
  public mOpcoes: CookieOptions = {
    path: "/"
  };

  constructor(public mCookieService: CookieService) {
    this.mAutenticacao = <Autenticacao>this.mCookieService.getObject(this.NOME_STORANGE);
  }

  autenticar(dados: Autenticacao): void {
    if (this.autenticado)
      throw new Error("Já está autenticado.");

    this.mCookieService.putObject(this.NOME_STORANGE, dados, this.mOpcoes);
    this.mAutenticacao = dados;
  }

  desautenticar(): void {

    if (!this.autenticado)
      throw new Error("Não autenticado.");

    this.mCookieService.remove(this.NOME_STORANGE, this.mOpcoes);
    this.mAutenticacao = null;
  }

  get autenticado(): boolean {
    return this.mAutenticacao != null;
  }

  get dadosAutenticacao(): Autenticacao {

    if (this.autenticado) {
      return this.mAutenticacao;
    }
    else
      return null;
  }
}

export class Usuario {
  DataCriacao: Date;
  Id: number;
  Nome: string;
  NomeUsuario: string;
}

export class Autenticacao {
  usuario: Usuario;
  token: string;
}
