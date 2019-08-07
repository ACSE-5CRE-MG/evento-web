import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { CaixaMensagem, CaixaMensagemBotoes, CaixaMensagemEstilos, EnumTipoAlerta } from './caixa-mensagem-dlg';
import { Observable } from 'rxjs';

export class Alertas {

    constructor(private dialogo: MatDialog) {
  }

  alertarErro(mensagem: string): void {
    this.alertar('Erro', mensagem, "", CaixaMensagemBotoes.Ok, CaixaMensagemEstilos.Completo, EnumTipoAlerta.Erro);
  }

  private alertar(titulo: string, mensagem: string, informacao: string, botoes: CaixaMensagemBotoes, estilo: CaixaMensagemEstilos, tipo: EnumTipoAlerta): Observable<any> {
    return CaixaMensagem.apresentar(this.dialogo, mensagem, titulo, informacao,
      botoes, false, estilo, "350px", tipo);
  }

  alertarAtencao(mensagem: string, detalhes: string): void {
    this.alertar('Atenção', mensagem, detalhes, CaixaMensagemBotoes.Ok, CaixaMensagemEstilos.Completo, EnumTipoAlerta.Atencao);
  }

  alertarInformacao(mensagem: string, detalhes: string): void {
    this.alertar('Informação', mensagem, detalhes, CaixaMensagemBotoes.Ok, CaixaMensagemEstilos.Completo, EnumTipoAlerta.Informacao);
  }
    
  alertarConfirmacao(mensagem: string, detalhes: string): Observable<any> {
    return this.alertar('Confirmação', mensagem, detalhes, CaixaMensagemBotoes.SimNao, CaixaMensagemEstilos.Completo, EnumTipoAlerta.Confirmacao);
  }

  alertarProcessamento(mensagem: string): MatDialogRef<DlgEmProcessamento> {

    return this.dialogo.open<DlgEmProcessamento>(DlgEmProcessamento, {
      width: '300px',
      disableClose: true,
      data: {
        texto: mensagem
      }
    });
  }
}

@Component({
  selector: 'dlg-processando',
  templateUrl: './dlg-processando.html',
  styles: [':host { margin: 10px }' ]
})
export class DlgEmProcessamento {

    constructor(public dialogRef: MatDialogRef<DlgEmProcessamento>, @Inject(MAT_DIALOG_DATA) public data: any) { }
}
