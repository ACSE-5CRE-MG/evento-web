import { Injectable } from '@angular/core';

@Injectable()
export class GestaoAutenticacao {

    public NOME_STORANGE: string = "eventoweb-autenticacao";
    public mAutenticacao: Usuario = null;

    constructor() {
        this.mAutenticacao = <Usuario>localStorage.getObject(this.NOME_STORANGE);
    }

    autenticar(dados: Usuario): void {
        if (this.autenticado)
            throw new Error("Já está autenticado.");

        localStorage.setItem(this.NOME_STORANGE, JSON.stringify(dados));
        this.mAutenticacao = dados;
    }

    desautenticar(): void {

        if (!this.autenticado)
            throw new Error("Não autenticado.");

        localStorage.removeItem(this.NOME_STORANGE);
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
