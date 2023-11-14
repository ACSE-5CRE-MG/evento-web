import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { GestaoAutenticacao } from "../../seguranca/gestao-autenticacao";
import { DialogoSenhaAdmin } from "../../usuarios/senha-admin/dlg-form-senha-admin";
import { DialogoSenhaComum } from "../../usuarios/senha-comum/dlg-form-senha-comum";
import { WebServiceAutenticacao } from "../../webservices/webservice-autenticacao";
import { Alertas } from "../alertas-dlg/alertas";

@Component({
  selector: 'menu-usuario',
  templateUrl: './menu-usuario.html',
})
export class MenuUsuario {

  constructor(
    public gestaoAutenticacao: GestaoAutenticacao,
    private router: Router,
    private wsAutenticacao: WebServiceAutenticacao,
    private alertas: Alertas,
    private dlgAlteracaoSenha: DialogoSenhaComum,
    private dlg2: DialogoSenhaAdmin) { }

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

  clicarAlterarDados(): void {
    this.dlg2.apresentar("admin");
  }

  clicarAlterarSenha(): void {
    this.dlgAlteracaoSenha.apresentar();
  }
}
