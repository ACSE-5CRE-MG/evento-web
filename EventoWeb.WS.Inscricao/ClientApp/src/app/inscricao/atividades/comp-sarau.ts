import { Component, EventEmitter, Input, Output, ViewChild, Inject, Injectable } from '@angular/core';
import { DTOSarau, DTOInscricaoSimplificada } from '../objetos';
import { CoordenacaoCentral } from '../../componentes/central/coordenacao-central';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { Observable } from 'rxjs';
import { WsManutencaoInscricoes } from '../../webservices/wsManutencaoInscricoes';


@Component({
    selector: 'comp-sarau',
    templateUrl: './comp-sarau.html'
})
export class ComponenteSarau {

    @Input()
    valor: DTOSarau[];
    @Output()
    valorChange: EventEmitter<DTOSarau[]> = new EventEmitter<DTOSarau[]>();

    @Input()
    inscrito: DTOInscricaoSimplificada;

    constructor(private coordenacao: CoordenacaoCentral, private DlgsSarau: DialogosSarau, private wsManInscricoes: WsManutencaoInscricoes) { }


    clicarCodigo(): void {
        this.DlgsSarau.apresentarDlgCodigo()
            .subscribe(
                (codigo) => {
                    if (codigo != null) {

                        let dlg = this.coordenacao.Alertas.alertarProcessamento("Buscando sarau pelo código...");

                        this.wsManInscricoes.obterSarau(this.inscrito.IdEvento, this.inscrito.Id, codigo)
                            .subscribe(
                                (sarau) => {
                                    sarau.Participantes.push(this.inscrito);
                                    this.valor.push(sarau);
                                    this.valorChange.emit(this.valor);
                                },
                                (erro) => {
                                    dlg.close();
                                    this.coordenacao.ProcessamentoErro.processar(erro);
                                }
                            );
                    }
                }
            );
    }

    clicarCriar(): void {
        this.DlgsSarau.apresentarDlgForm(null)
            .subscribe(
                (sarauCriado) => {
                    if (sarauCriado != null) {
                        sarauCriado.Participantes = [];
                        sarauCriado.Participantes.push(this.inscrito);

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

    gerarTextoParticipantes(sarau: DTOSarau): string {

        let pessoas: string = "";
        for (let inscricao of sarau.Participantes) {

            if (pessoas != "")
                pessoas = pessoas + ", ";

            if (inscricao.Id == this.inscrito.Id)
                pessoas = pessoas + "Você";
            else
                pessoas = pessoas + inscricao.Nome;
        }

        return pessoas;
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
    tituloDlg: string;

    constructor(private dialogRef: MatDialogRef<DlgSarauFormulario>,
        @Inject(MAT_DIALOG_DATA) public data: DTOSarau) {

        this.tituloDlg = "Nova Apresentação";
        if (data == null)
            this.sarau = new DTOSarau();
        else {
            this.sarau = data;
            this.tituloDlg = "Alteração de Apresentação";
        }
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
