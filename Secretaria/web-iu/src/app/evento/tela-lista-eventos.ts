import { Component, OnInit } from '@angular/core';
import { WebServiceEventos } from '../webservices/webservice-eventos';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { DTOEventoMinimo, DTOEvento } from './objetos';
import { ServicoDlgFormEvento } from './dlg-form-evento';

@Component({
  selector: 'tela-lista-eventos',
  templateUrl: './tela-lista-eventos.html',
})
export class TelaListaEventos implements OnInit {

  eventos: DTOEventoMinimo[][];

  constructor(public wsEventos: WebServiceEventos, public alertas: Alertas, public srv: ServicoDlgFormEvento) { }

  ngOnInit(): void {

    this.eventos = [];

    //let dlg = this.alertas.alertarProcessamento("Buscando eventos...");

    this.wsEventos.obterTodos()
      .subscribe(eventosRetornados => {

        let linhas = Math.floor(eventosRetornados.length / 3);
        if (eventosRetornados.length % 3 != 0)
          linhas++;

        let indiceEventos = 0;
        for (let indiceLinha = 0; indiceLinha < linhas; indiceLinha++) {

          let linhaEventos = [];       

          for (let IndiceCol = 0; IndiceCol < 3 && indiceEventos < eventosRetornados.length; IndiceCol++) {
            linhaEventos.push(eventosRetornados[indiceEventos]);
            indiceEventos++;
          }

          this.eventos.push(linhaEventos);
        }

        //dlg.close();
      },
      erro => {
        this.alertas.alertarErro(erro);
       // dlg.close();
      });
  }

  clicarNovo(): void {
    this.srv.abrir();
  }
}
