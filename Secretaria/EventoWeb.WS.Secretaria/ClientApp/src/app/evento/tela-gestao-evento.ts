import { Component, OnInit } from '@angular/core';
import { WebServiceEventos } from '../webservices/webservice-eventos';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ActivatedRoute } from '@angular/router';
import { DTOEventoCompleto } from './objetos';
import { WebServiceRelatorios } from '../webservices/webservice-relatorios';

@Component({
  selector: 'tela-gestao-evento',
  styleUrls: ['./tela-gestao-evento.scss'],
  templateUrl: './tela-gestao-evento.html'
})
export class TelaGestaoEvento implements OnInit {

  evento: DTOEventoCompleto = new DTOEventoCompleto();

  constructor(public wsEventos: WebServiceEventos, public mensageria: Alertas, public roteador: ActivatedRoute,
    private wsRelatorios: WebServiceRelatorios) { }

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

  clicarInscritosDepartamentos(): void {
    let dlg = this.mensageria.alertarProcessamento("Gerando relatório inscritos departamentos...");

    this.wsRelatorios.obterInscritosDepartamentos(this.evento.Id)
      .subscribe(
        relatorioGerado => {
          dlg.close();
          window.open(URL.createObjectURL(relatorioGerado), '_blank');
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        }
      );
  }
}
