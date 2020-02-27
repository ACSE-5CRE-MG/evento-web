import { Component, ViewChild, Injectable } from '@angular/core';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { Observable } from 'rxjs';

@Component({
  selector: 'dlg-inscricao-adulto-codigo',
  templateUrl: './dlg-inscricao-adulto-codigo.html'
})
export class DlgInscricaoAdultoCodigo {

  codigo: string;

  @ViewChild("grupoValidacao")
  grupoValidacao: DxValidationGroupComponent;

  constructor(private dialogRef: MatDialogRef<DlgInscricaoAdultoCodigo>) { }

  clicarOK(): void {
    let validacao = this.grupoValidacao.instance.validate();
    if (validacao.isValid)
      this.dialogRef.close(this.codigo);
  }

  clicarCancelar(): void {
    this.dialogRef.close();
  }
}

@Injectable()
export class DialogosInscricao {
  constructor(private srvDialog: MatDialog) { }

  apresentarDlgCodigo(): Observable<string> {
    const dlg = this.srvDialog.open(DlgInscricaoAdultoCodigo);
    return dlg.afterClosed();
  }
}
