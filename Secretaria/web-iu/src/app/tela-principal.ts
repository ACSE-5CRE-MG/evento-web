import { Component } from '@angular/core';

@Component({
  selector: 'tela-principal',
  templateUrl: './tela_principal.html'
})

export class TelaPrincipal {

  estaAutenticado: boolean = false;

  processarLoginAutenticado(autenticacao: any): void {

  }
}
