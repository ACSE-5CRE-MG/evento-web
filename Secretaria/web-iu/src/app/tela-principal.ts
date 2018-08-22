import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'tela-principal',
  templateUrl: './tela-principal.html'
})

export class TelaPrincipal {

  constructor(public router: Router) {}

}
