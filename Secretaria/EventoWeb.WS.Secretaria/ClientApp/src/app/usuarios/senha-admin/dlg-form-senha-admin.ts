import { Component, Inject, Injectable, ViewChild } from "@angular/core";
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from "@angular/material";
import { Alertas } from "../../componentes/alertas-dlg/alertas";
import { Observable } from 'rxjs';
import { DxValidationGroupComponent } from "devextreme-angular/ui/validation-group";
import { DTOAlteracaoSenhaWS } from "../objetos";
import { WebServiceUsuariosAdm } from "../../webservices/webservice-usuarios-adm";

@Component({
  selector: 'dlg-form-senha-admin',
  styleUrls: ['./dlg-form-senha-admin.scss'],
  templateUrl: './dlg-form-senha-admin.html'
})
export class DlgFormSenhaAdmin {    

  novaSenha: string
  repeticaoNovaSenha: string;

  @ViewChild("grupoValidacao", { static: true })
  grupoValidacao: DxValidationGroupComponent;

  private login: string;

  constructor(
    public dialogRef: MatDialogRef<DlgFormSenhaAdmin>,
    private alertas: Alertas,
    private wsUsuarios: WebServiceUsuariosAdm,
    @Inject(MAT_DIALOG_DATA) private data: any) {

    if (data != null)
      this.login = data.login || "";
  }

  comparadorSenha = () => {
    return this.novaSenha;
  };

  clicarSalvar(): void {
    if (this.grupoValidacao.instance.validate().isValid) {
      let dlg = this.alertas.alertarProcessamento("Alterando senha, aguarde...");

      let dto = new DTOAlteracaoSenhaWS();
      dto.NovaSenha = this.novaSenha;
      dto.NovaSenhaRepetida = this.repeticaoNovaSenha;

      this.wsUsuarios.atualizarSenha(this.login, dto)
        .subscribe(
          () => {            
            dlg.close();
            this.dialogRef.close();
          },
          erro => {
            dlg.close();
            this.alertas.alertarErro(erro);
          }
        );
    }
  }

  clicarCancelar() {
    this.dialogRef.close();
  }  
}

@Injectable()
export class DialogoSenhaAdmin {

  constructor(private srvDialog: MatDialog) { }

  apresentar(login: string): Observable<void> {
    const dlg = this.srvDialog.open(DlgFormSenhaAdmin, { data: { login: login }, width: '380px' });
    return dlg.afterClosed();
  }
}
