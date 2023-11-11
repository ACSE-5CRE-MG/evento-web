import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GestaoAutenticacao } from '../seguranca/gestao-autenticacao';
import { WebServiceAutenticacao } from '../webservices/webservice-autenticacao';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { DTWDadosAutenticacao } from '../seguranca/objetos';

@Component({
  selector: 'tela-login',
  templateUrl: './tela-login.html',
  styleUrls: ['./tela-login.scss']
})

export class TelaLogin implements OnInit {

  paraOndeRedirecionar: string;
  nomeUsuario: string;
  senha: string;

  constructor(public gestaoAutenticacao: GestaoAutenticacao,
    public router: Router,
    public route: ActivatedRoute,
    public wsAutenticacao: WebServiceAutenticacao,
    public alertas: Alertas) {

  }

  ngOnInit() {
    this.paraOndeRedirecionar = this.route.snapshot.queryParams['urlRetornar'] || '/';

    if (this.gestaoAutenticacao.autenticado)
      this.router.navigate([this.paraOndeRedirecionar]);
  }

  clicarAutenticar(): void {

    let dlg = this.alertas.alertarProcessamento("Autenticando...");

    let dto = new DTWDadosAutenticacao();
    dto.Login = this.nomeUsuario;
    dto.Senha = this.senha;

    this.wsAutenticacao.autenticar(dto)
      .subscribe(dadosAutenticacao => {
        this.gestaoAutenticacao.autenticar(dadosAutenticacao);
        this.router.navigate([this.paraOndeRedirecionar]);
        dlg.close();
      },
        dadosErro => {
          this.alertas.alertarErro(dadosErro);
          dlg.close();
        });
  }
}
