import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { GestaoAutenticacao } from '../../seguranca/gestao-autenticacao';
import { WebServiceAutenticacao } from '../../webservices/webservice-autenticacao';
import { Alertas } from '../../componentes/alertas-dlg/alertas';

@Component({
  selector: 'layout-base',
  styleUrls: ['./layout-base.scss'],
  templateUrl: './layout-base.html',
})
export class LayoutBase {

  @Input('corToolbar') corToolbar: string = 'primary';

  constructor(public router: Router, public gestaoAutenticacao: GestaoAutenticacao,
    public wsAutenticacao: WebServiceAutenticacao,
    public alertas: Alertas) { }

  clicarSair(): void {

    let dlg = this.alertas.alertarProcessamento("Autenticando...");

    this.wsAutenticacao.desautenticar()
      .subscribe(retorno => {
        this.gestaoAutenticacao.desautenticar();
        this.router.navigate(['login']);
        dlg.close();
      },
        dadosErro => {
          this.alertas.alertarErro(dadosErro);
          dlg.close();
        });

  }
}
