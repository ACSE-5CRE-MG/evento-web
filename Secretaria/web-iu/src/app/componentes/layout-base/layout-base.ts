import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { GestaoAutenticacao } from '../../seguranca/gestao-autenticacao';

@Component({
  selector: 'layout-base',
  styleUrls: ['./layout-base.scss'],
  templateUrl: './layout-base.html',
})
export class LayoutBase {

  @Input('corToolbar') corToolbar: string = 'primary';

  constructor(public router: Router, public gestaoAutenticacao: GestaoAutenticacao) {}

  clicarSair(): void {
    this.gestaoAutenticacao.desautenticar();
    this.router.navigate(['login']);
  }
}
