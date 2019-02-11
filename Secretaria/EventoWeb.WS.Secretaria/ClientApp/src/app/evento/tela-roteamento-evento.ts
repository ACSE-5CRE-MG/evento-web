import { Component, OnInit, OnDestroy } from '@angular/core';
import { WebServiceEventos } from '../webservices/webservice-eventos';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ActivatedRoute } from '@angular/router';
import { DTOEventoCompleto } from './objetos';
import { Subscription } from 'rxjs';

@Component({
  selector: 'tela-roteamento-evento',
  templateUrl: './tela-roteamento-evento.html'
})
export class TelaRoteamentoEvento implements OnInit, OnDestroy {

  evento: DTOEventoCompleto = new DTOEventoCompleto();
  parametrosPag: Subscription;

  constructor(public wsEventos: WebServiceEventos, public mensageria: Alertas, public roteador: ActivatedRoute) { }

  ngOnInit(): void {
    this.parametrosPag = this.roteador.params.subscribe(parametros => {
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

  ngOnDestroy(): void {
    this.parametrosPag.unsubscribe();
  }

  clicarEditar(): void {

  }
}
