import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { GestaoAutenticacao } from "../../seguranca/gestao-autenticacao";
import { WebServiceAutenticacao } from "../../webservices/webservice-autenticacao";
import { Alertas } from "../alertas-dlg/alertas";

@Component({
  selector: 'menu-usuario',
  templateUrl: './menu-usuario.html',
})
export class MenuUsuario {

  constructor(public router: Router, public gestaoAutenticacao: GestaoAutenticacao,
    public wsAutenticacao: WebServiceAutenticacao,
    public alertas: Alertas) { }

  get ehAdmin() {
    return this.gestaoAutenticacao.autenticado &&
      this.gestaoAutenticacao.dadosAutenticacao.Usuario.EhAdministrador;
  }

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
