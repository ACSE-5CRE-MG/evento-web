import { Component, OnInit } from '@angular/core';
import { WebServiceEventos } from '../webservices/webservice-eventos';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ActivatedRoute } from '@angular/router';
import { DTOEventoCompleto } from './objetos';

@Component({
  selector: 'tela-gestao-evento',
  templateUrl: './tela-gestao-evento.html'
})
export class TelaGestaoEvento implements OnInit {

  evento: DTOEventoCompleto = new DTOEventoCompleto();

  constructor(public wsEventos: WebServiceEventos, public mensageria: Alertas, public roteador: ActivatedRoute) { }

  ngOnInit(): void {
    this.roteador.params.subscribe(parametros => {
      let dlg = this.mensageria.alertarProcessamento("Buscando dados do evento");

      this.wsEventos.obterId(+parametros["id"])
        .subscribe(evento => {
          this.evento = evento;
          dlg.close();
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        });
    });
  }

  clicarEditar(): void {

  }
}
