/*import { Component, Inject, OnInit } from "@angular/core";
import { WebServiceSala } from "../webservices/webservice-salas";
import { DTOSalaEstudo } from "./objetos";

import createNumberMask from 'text-mask-addons/dist/createNumberMask';
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { Alertas } from "../componentes/alertas-dlg/alertas";

@Component({
  selector: 'dlg-form-sala',
  //styleUrls: ['./dlg-form-sala.scss'],
  templateUrl: './dlg-form-sala.html'
})
export class DlgFormSala implements OnInit {    

  sala: DTOSalaEstudo;
  idSala: number;
  idEvento: number;
  usaFaixaEtaria: boolean;
  podeUsarFaixaEtaria: boolean;
  titulo: string;

  mascaraNumero = {
    mask: createNumberMask({
      prefix: '',
      suffix: '',
      thousandsSeparatorSymbol: '.',
      allowDecimal: false,
      allowNegative: false
    })
  };

  constructor(public dialogRef: MatDialogRef<DlgFormSala>, public alertas: Alertas,
    public wsSalas: WebServiceSala, @Inject(MAT_DIALOG_DATA) public data: any) {

    this.podeUsarFaixaEtaria = false;
    this.idSala = null;
    this.idEvento = null;

    if (data != null) {
      this.idSala = data.Id || null;
      this.podeUsarFaixaEtaria = data.podeUsarFaixaEtaria || false;
      this.idEvento = data.idEvento || 0;
    }
  }

  ngOnInit(): void {
    this.usaFaixaEtaria = false;
    this.titulo = "Inclusão de Sala de Estudo";

    if (this.idSala == null)
      this.sala = new DTOSalaEstudo();
    else {
      this.titulo = "Atualização de Sala de Estudo";
      let dlg = this.alertas.alertarProcessamento("Buscando dados da Sala...");
      this.wsSalas.obterId(this.idSala)
        .subscribe(
          sala => {
            this.sala = sala;
            this.usaFaixaEtaria = this.sala.IdadeMaxima != null && this.sala.IdadeMinima != null;
            dlg.close();
          },
          erro => {
            dlg.close();
            this.alertas.alertarErro(erro);
          }
        );
    }
  }

  clicarSalvar(): void {
    if (this.sala.Nome == null || this.sala.Nome.trim().length == 0)
      this.alertas.alertarAtencao('Informe o nome da sala.', "");
    else if (this.podeUsarFaixaEtaria && this.usaFaixaEtaria && (this.sala.IdadeMinima == null || this.sala.IdadeMaxima == null))
      this.alertas.alertarAtencao('Você precisa informar a faixa etária inicial e final', '');
    else if (this.podeUsarFaixaEtaria && this.usaFaixaEtaria && this.sala.IdadeMinima > this.sala.IdadeMaxima)
      this.alertas.alertarAtencao('A idade mínima deve ser menor ou igual a máxima', '');
    else {
      let dlg = this.alertas.alertarProcessamento("Salvando Sala, aguarde...");

      let servico: any;

      if (this.idSala == null)
        servico = this.wsSalas.incluir(this.idEvento, this.sala);
      else
        servico = this.wsSalas.atualizar(this.idSala, this.sala);

      servico
        .subscribe(
          retorno => {            
            dlg.close();
            this.dialogRef.close(retorno.Id || null);
          },
          erro => {
            dlg.close();
            this.alertas.alertarErro(erro);
          }
        );
    }
  }
}*/
