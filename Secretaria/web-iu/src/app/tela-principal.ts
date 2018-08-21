import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GestaoAutenticacao } from './seguranca/gestao-autenticacao';

@Component({
  selector: 'tela-principal',
  templateUrl: './tela-principal.html',
  styleUrls: ['./tela-principal.scss']
})

export class TelaPrincipal {

  constructor(public router: Router, public gestaoAutenticacao: GestaoAutenticacao) {}

  clicarSair(): void {
    this.gestaoAutenticacao.desautenticar();
    this.router.navigate(['login']);
  }
}
