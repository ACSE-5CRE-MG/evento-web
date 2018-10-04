import { Component, Injectable, OnInit } from "@angular/core";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";

import createNumberMask from 'text-mask-addons/dist/createNumberMask';
import { DTOEventoCompleto } from "./objetos";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { WebServiceEventos } from "../webservices/webservice-eventos";

@Component({
  selector: "dlg-form-evento",
  templateUrl: "./dlg-form-evento.html",
  styleUrls: ["./dlg-form-evento.scss"]
})
export class DlgFormEvento implements OnInit {

  titulo: string = "Inclusão de Evento";
  
  mascaraHora = {
    mask: [/\d/, /\d/, ':', /\d/, /\d/],
    guide: true
  };

  mascaraNumero = {
    mask: createNumberMask({
      prefix: '',
      suffix: '',
      thousandsSeparatorSymbol: '.',
      allowDecimal: false,
      allowNegative: false
    })
  };

  evento: DTOEventoCompleto;

  constructor(
    public dialogRef: MatDialogRef<DlgFormEvento>, public alertas: Alertas,
    public wsEventos: WebServiceEventos) { }

  ngOnInit(): void {
    this.evento = new DTOEventoCompleto();
    this.evento.DataRegistro = new Date();
  }

  clicarCarregarImagem(): void {

  }

  clicarLimparImagem(): void {

  }

  clicarSalvar(): void {
    if (this.evento.Nome == null || this.evento.Nome.trim().length == 0)
      this.alertas.alertarAtencao("Você não informou o nome do evento", "");
    else if (this.evento.DataInicioEvento == null)
      this.alertas.alertarAtencao("Você não informou a data que inicia o evento", "");
    else if (this.evento.DataFimEvento == null)
      this.alertas.alertarAtencao("Você não informou a data que termina o evento", "");
    else if (this.evento.DataInicioInscricao == null)
      this.alertas.alertarAtencao("Você não informou a data que inicia as inscrições", "");
    else if (this.evento.DataFimInscricao == null)
      this.alertas.alertarAtencao("Você não informou a data que termina as inscrições", "");
    else if (this.evento.TemEvangelizacao && this.evento.PublicoEvangelizacao == null)
      this.alertas.alertarAtencao("Você precisa dizer qual o público da evangelização", "");
    else if (this.evento.TemSalasEstudo && this.evento.ModeloDivisaoSalaEstudo == null)
      this.alertas.alertarAtencao("Você precisa dizer qual o modelo de divisão da sala de estudo", "");
    else if (this.evento.TemSarau && (this.evento.TempoDuracaoSarauMin == null || this.evento.TempoDuracaoSarauMin == 0))
      this.alertas.alertarAtencao("Você precisa dizer o tempo do duração. Esse tempo deve ser maior que zero", "");
    else {
      let dlg = this.alertas.alertarProcessamento("Salvando evento, aguarde...");
      this.wsEventos
        .incluir(this.evento)
        .subscribe(
          (retornoOK) => {
            dlg.close();
            this.dialogRef.close();
          },
          (retornoErro) => {
            dlg.close();
            this.alertas.alertarErro(retornoErro);
          });
    }
  }
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
