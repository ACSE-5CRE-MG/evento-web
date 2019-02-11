import { Component, OnInit } from "@angular/core";
import { DTOEventoCompleto } from "../evento/objetos";
import { WebServiceEventos } from "../webservices/webservice-eventos";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { ActivatedRoute } from "@angular/router";
import { WebServiceSala } from "../webservices/webservice-salas";
import { DTOSalaEstudo } from "./objetos";


@Component({
  selector: 'tela-listagem-salas',
  styleUrls: ['./tela-listagem-salas.scss'],
  templateUrl: './tela-listagem-salas.html'
})
export class TelaListagemSalas implements OnInit {

  evento: DTOEventoCompleto = new DTOEventoCompleto();
  salaSelecionada: DTOSalaEstudo = null;
  salas: DTOSalaEstudo[] = [];

  agGridSala: any;

  definicaoColunas = [
    { headerName: 'Id', field: 'Id', filter: true },
    { headerName: 'Nome', field: 'Nome', filter: true },
    { headerName: 'Total Par?', field: 'DeveSerParNumeroTotalParticipantes', filter: true },
    { headerName: 'Faixa Etária', field: 'IdadeMinima', filter: true }
  ];    

  constructor(public wsSalas: WebServiceSala, public mensageria: Alertas, public roteador: ActivatedRoute) { }

  ngOnInit(): void {
    this.roteador.parent.params.subscribe(parametros => {
      let dlg = this.mensageria.alertarProcessamento("Buscando salas...");

      this.wsSalas.obterTodas(+parametros["id"])
        .subscribe(salas => {
          this.salas = salas;
          dlg.close();
        },
          erro => {
            dlg.close();
            this.mensageria.alertarErro(erro);
          });
    });
  }

  clicarIncluir(): void {

  }

  clicarEditar(): void {

  }

  clicarExcluir(): void {

  }

  realizarQdoSelecaoAgGridMudar(): void {
    var registrosSelecionado = this.agGridSala.getSelectedRows();
    if (registrosSelecionado != null && registrosSelecionado.length > 0)
      this.salaSelecionada = registrosSelecionado[0];
  }

  realizarQdoAgGridPronto(evento: any): void {
    this.agGridSala = evento.api;
    evento.api.sizeColumnsToFit();
  }
}
