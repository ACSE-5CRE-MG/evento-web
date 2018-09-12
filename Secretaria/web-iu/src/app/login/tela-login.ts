import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GestaoAutenticacao } from '../seguranca/gestao-autenticacao';
import { WebServiceAutenticacao } from '../webservices/webservice-autenticacao';
import { Alertas } from '../componentes/alertas-dlg/alertas';

@Component({
  selector: 'tela-login',
  templateUrl: './tela-login.html',
  styleUrls: ['./tela-login.scss']
})

export class TelaLogin implements OnInit {

  paraOndeRedirecionar: string;

  constructor(public gestaoAutenticacao: GestaoAutenticacao,
    public router: Router,
    public route: ActivatedRoute,
    public wsAutenticacao: WebServiceAutenticacao,
    public alertas: Alertas) {

  }

  ngOnInit() {
    this.paraOndeRedirecionar = this.route.snapshot.queryParams['urlRetornar'] || '/';
  }

  clicarAutenticar(): void {

    let dlg = this.alertas.alertarProcessamento("Autenticando...");
    this.wsAutenticacao.autenticarSemFacebook("robsonmbobbi@gmail.com", "EVENTOWEB-0192")
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
