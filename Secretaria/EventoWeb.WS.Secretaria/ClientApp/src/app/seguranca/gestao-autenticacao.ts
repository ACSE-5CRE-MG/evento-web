import { Injectable } from '@angular/core';
import { CookieService, CookieOptions } from 'ngx-cookie';

@Injectable()
export class GestaoAutenticacao {

  public NOME_STORANGE: string = "eventoweb-autenticacao";
  public mAutenticacao: Usuario = null;
  public mOpcoes: CookieOptions = {
    path: "/"
  };

  constructor(public mCookieService: CookieService) {
    this.mAutenticacao = <Usuario>this.mCookieService.getObject(this.NOME_STORANGE);
  }

  autenticar(dados: Usuario): void {
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

  get dadosAutenticacao(): Usuario {

    if (this.autenticado) {
      return this.mAutenticacao;
    }
    else
      return null;
  }
}

export class Usuario {
  email: string;
  name: string;
  picture: Data;
  imagem64: string;
  TokenApi: string;
}

export class Data {
  data: ImgUsuario;
}

export class ImgUsuario {
  is_silhouette: boolean;
  url: string;
}
