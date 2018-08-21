import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GestaoAutenticacao, Autenticacao, Usuario } from '../seguranca/gestao-autenticacao';

@Component({
  selector: 'tela-login',
  templateUrl: './tela-login.html',
  styleUrls: ['./tela-login.scss']
})

export class TelaLogin {

  nomeUsuario: string;
  senha: string;

  constructor(public gestaoAutenticacao: GestaoAutenticacao, public router: Router) {

  }

  clicarAutenticar(): void {
    let usuario = new Usuario();
    
    usuario.DataCriacao = new Date();
    usuario.Id = -1;
    usuario.Nome = 'Usuario de Teste';
    usuario.NomeUsuario = 'UsuarioTeste';

    let autenticacao = new Autenticacao();
    autenticacao.usuario = usuario;
    autenticacao.token = '99999';

    this.gestaoAutenticacao.autenticar(autenticacao);
    
    this.router.navigate([''])
  }
}
