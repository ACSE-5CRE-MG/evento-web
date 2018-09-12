import { Injectable, Component, ViewContainerRef, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { CaixaMensagem, CaixaMensagemBotoes, CaixaMensagemEstilos } from './caixa-mensagem-dlg';
import { Observable } from 'rxjs';

@Injectable()
export class Alertas {

    constructor(public dialogo: MatDialog) {
  }

  alertarErro(mensagem: any): void {        
    let me = this;
    
    if (typeof mensagem == "object") {
      var fileReader = new FileReader();
      fileReader.onload = function () {

        console.log(typeof this.result);
        let erro = JSON.parse(this.result);
        me.alertar('Erro', (erro.ExceptionMessage ? erro.ExceptionMessage : this.result), "", CaixaMensagemBotoes.Ok, CaixaMensagemEstilos.Completo);
      };
      fileReader.readAsText(<Blob>mensagem);
    }
    else {
      this.alertar('Erro', mensagem.toString(), "", CaixaMensagemBotoes.Ok, CaixaMensagemEstilos.Completo);
    }
  }

  alertar(titulo: string, mensagem: string, informacao: string, botoes: CaixaMensagemBotoes, estilo: CaixaMensagemEstilos): Observable<any> {
    return CaixaMensagem.apresentar(this.dialogo, mensagem, titulo, informacao,
      botoes, false, estilo, "350px");
  }

  alertarAtencao(mensagem: string, detalhes: string): void {
    this.alertar('Atenção', mensagem, detalhes, CaixaMensagemBotoes.Ok, CaixaMensagemEstilos.Completo);
  }

  alertarInformacao(mensagem: string, detalhes: string): void {
    this.alertar('Informação', mensagem, detalhes, CaixaMensagemBotoes.Ok, CaixaMensagemEstilos.Completo);
  }
    
  alertarConfirmacao(mensagem: string, detalhes: string): Observable<any> {
    return this.alertar('Confirmação', mensagem, detalhes, CaixaMensagemBotoes.SimNao, CaixaMensagemEstilos.Completo);
  }

  alertarProcessamento(mensagem: string): MatDialogRef<DlgEmProcessamento> {

    return this.dialogo.open<DlgEmProcessamento>(DlgEmProcessamento, {
      //width: '650px',
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
})
export class DlgEmProcessamento {

    constructor(public dialogRef: MatDialogRef<DlgEmProcessamento>, @Inject(MAT_DIALOG_DATA) public data: any) { }
}
