import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'tela-login',
  templateUrl: './tela-login.html'
})

export class TelaLogin {

  nomeUsuario: string;
  senha: string;

  aoOcorrerAutenticacao: EventEmitter<any> = new EventEmitter<any>();

  clicarAutenticar() {

  }
}
