import { Component, Injectable } from "@angular/core";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";

// Origem https://github.com/trashvin/messagebox-like-angular-alertbox

@Component({
  selector: "dlg-form-evento",
  templateUrl: "./dlg-form-evento.html",
  styleUrls: ["./dlg-form-evento.scss"]
})
export class DlgFormEvento {
  titulo: string = "Inclusão de Evento";

  constructor(
    public dialogRef: MatDialogRef<DlgFormEvento>) {}
}

@Injectable()
export class ServicoDlgFormEvento {

  constructor(public dialog: MatDialog) { }

  abrir() {
    const dialogRef = this.dialog.open(DlgFormEvento, {
      width: '100vw',
      height: '100vh',
      maxWidth: '100vw',
      maxHeight: '100vh'
    });
  }
}
