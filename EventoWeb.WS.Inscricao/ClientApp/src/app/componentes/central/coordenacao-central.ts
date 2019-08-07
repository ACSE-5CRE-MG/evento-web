import { Injectable } from "@angular/core";
import { ServicoProcessamentoErros } from "./servico-processamento-erros";
import { Alertas } from "../alertas-dlg/alertas";
import { MatDialog } from "@angular/material";
import { Router } from "@angular/router";

@Injectable()
export class CoordenacaoCentral {

  private mProcessamentoErro: ServicoProcessamentoErros;
  private mAlertas: Alertas;
  private mFormatacoes: Formatacoes;

  public get ProcessamentoErro(): ServicoProcessamentoErros { return this.mProcessamentoErro; }
  public get Alertas(): Alertas { return this.mAlertas; }
  public get Formatacoes(): Formatacoes { return this.mFormatacoes; }

  constructor(private dialogo: MatDialog, private roteador: Router) {

    this.mAlertas = new Alertas(dialogo);
    this.mProcessamentoErro = new ServicoProcessamentoErros(this.mAlertas, roteador);
    this.mFormatacoes = new Formatacoes();
  }
}

export class Formatacoes {

  public get FormatoDataHora(): string
  {
      return "dd/MM/yyyy HH:mm";
  }

  public get FormatoData(): string
  {
      return "dd/MM/yyyy";
  }

  public get FormatoHora(): string
  {
     return "HH:mm";
  }
        
  public get FormatoDiaMesJuntos(): string
  {
      return "dd \\de MMMM \\de yyyy";
  }

  public get FormatoNumerico15por4(): string
  {
      return "###,###,###,###,##0.0000";
  }

  public get FormatoNumerico15por2(): string
  {
      return "###,###,###,###,##0.00";
  }

  public get FormatoPrecisaoCasasDecimais(): string
  {
      return "0.################";
  }

  public get FormatoInteiro6Casas(): string
  {
      return "000000";
  }
}
