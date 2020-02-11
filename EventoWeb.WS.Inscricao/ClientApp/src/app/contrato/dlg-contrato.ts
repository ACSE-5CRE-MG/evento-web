import { Component, Inject, Injectable } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material";
import { Observable } from 'rxjs';

@Component({
  selector: 'dlg-contrato',
  styleUrls: ['./dlg-contrato.scss'],
  templateUrl: './dlg-contrato.html'
})
export class DlgContrato {    

  idEvento: number;

  constructor(public dialogRef: MatDialogRef<DlgContrato>, @Inject(MAT_DIALOG_DATA) public data: any) {

    if (data != null) {
      this.idEvento = data.idEvento;
    }
  }
}

@Injectable()
export class DialogoContrato {

  constructor(private srvDialog: MatDialog) { }

  apresentarDlgFormDialogoInclusao(idEvento: number, podeUsarFaixaEtaria: boolean): Observable<void> {
    const dlg = this.srvDialog.open(DlgContrato, { data: { idEvento: idEvento } });
    return dlg.afterClosed();
  }
}
