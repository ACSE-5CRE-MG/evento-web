import { Component, EventEmitter, Input, Output, ViewChild, Inject, Injectable } from '@angular/core';
import { DTOSarau } from '../objetos';
import { CoordenacaoCentral } from '../../componentes/central/coordenacao-central';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { Observable } from 'rxjs';


@Component({
    selector: 'comp-sarau',
    templateUrl: './comp-sarau.html'
})
export class ComponenteSarau {

    @Input()
    valor: DTOSarau[];
    @Output()
    valorChange: EventEmitter<DTOSarau[]> = new EventEmitter<DTOSarau[]>();

    constructor(private coordenacao: CoordenacaoCentral, private DlgsSarau: DialogosSarau) { }


    clicarCodigo(): void {
        this.DlgsSarau.apresentarDlgCodigo()
            .subscribe(
                (codigo) => {
                    if (codigo != null) {
                        // faz as buscas
                    }
                }
            );
    }

    clicarCriar(): void {
        this.DlgsSarau.apresentarDlgForm(null)
            .subscribe(
                (sarauCriado) => {
                    if (sarauCriado != null) {
                        this.valor.push(sarauCriado);
                        this.valorChange.emit(this.valor);
                    }
                }
            );
    }

    editar(sarau: DTOSarau): void {
        this.DlgsSarau.apresentarDlgForm(sarau)
            .subscribe(
                (sarauAlterado) => {
                    if (sarauAlterado != null) {
                        let indice = this.valor.findIndex(x => x.Id == sarauAlterado.Id);
                        if (indice != -1) {
                            this.valor[indice] = sarauAlterado;
                            this.valorChange.emit(this.valor);
                        }
                    }
                }
            );
    }

    excluir(sarau: DTOSarau): void {

        this.coordenacao.Alertas.alertarConfirmacao("Deseja excluir este sarau?", "Ao removê-lo, este sarau também será excluido dos outros participantes")
            .subscribe(
                (botaoPressionado) => {
                    if (botaoPressionado.result == "sim") {
                        this.valor = this.valor.filter(x => x.Id != sarau.Id);
                        this.valorChange.emit(this.valor);
                    }
                });
    }
}

@Component({
    selector: 'dlg-sarau-codigo',
    templateUrl: './dlg-sarau-codigo.html'
})
export class DlgSarauCodigo {

    codigo: string;

    @ViewChild("grupoValidacao", { static: false })
    grupoValidacao: DxValidationGroupComponent;

    constructor(private dialogRef: MatDialogRef<DlgSarauCodigo>) { }

    clicarOK(): void {
        let validacao = this.grupoValidacao.instance.validate();
        if (validacao.isValid)
            this.dialogRef.close(this.codigo);
    }

    clicarCancelar(): void {
        this.dialogRef.close();
    }
}

@Component({
    selector: 'dlg-sarau-form',
    templateUrl: './dlg-sarau-form.html'
})
export class DlgSarauFormulario {

    @ViewChild("grupoValidacao", { static: false })
    grupoValidacao: DxValidationGroupComponent;

    sarau: DTOSarau;

    constructor(private dialogRef: MatDialogRef<DlgSarauFormulario>,
        @Inject(MAT_DIALOG_DATA) public data: DTOSarau) {

        if (data == null)
            this.sarau = new DTOSarau();
        else
            this.sarau = data;
    }

    clicarOK(): void {
        let validacao = this.grupoValidacao.instance.validate();
        if (validacao.isValid)
            this.dialogRef.close(this.sarau);
    }

    clicarCancelar(): void {
        this.dialogRef.close();
    }
}

@Injectable()
export class DialogosSarau {
    constructor(private srvDialog: MatDialog) { }

    apresentarDlgCodigo(): Observable<string> {
        const dlg = this.srvDialog.open(DlgSarauCodigo);
        return dlg.afterClosed();
    }

    apresentarDlgForm(sarau: DTOSarau): Observable<DTOSarau> {
        const dlg = this.srvDialog.open(DlgSarauFormulario, { data: sarau });
        return dlg.afterClosed();
    }
}
