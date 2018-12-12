import { Component, OnInit } from '@angular/core';
import { WebServiceEventos } from '../webservices/webservice-eventos';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { DTOEventoMinimo } from './objetos';
import { ServicoDlgFormEvento } from './dlg-form-evento';

@Component({
  selector: 'tela-lista-eventos',
  templateUrl: './tela-lista-eventos.html',
  styleUrls: ["./tela-lista-eventos.scss"]
})
export class TelaListaEventos implements OnInit {

  eventos: DTOEventoMinimo[];

  constructor(public wsEventos: WebServiceEventos, public alertas: Alertas, public srv: ServicoDlgFormEvento) { }

  ngOnInit(): void {

    this.eventos = [];

    let dlg = this.alertas.alertarProcessamento("Buscando eventos...");

    this.wsEventos.obterTodos()
      .subscribe(eventosRetornados => {

        this.eventos = eventosRetornados;

        dlg.close();
      },
      erro => {
        dlg.close();
        this.alertas.alertarErro(erro);
      });
  }

  clicarNovo(): void {
    this.srv.abrir();
  }

  obterImagem(evento: DTOEventoMinimo): string {
    if (evento.logotipo == null || evento.logotipo.trim().length == 0)
      return 'assets/semimagem.jpg';
    else
      return 'data:image/jpg;base64,' + evento.logotipo;
  }

  clicarExcluir(evento: DTOEventoMinimo): void {
    this.alertas.alertarConfirmacao("Você deseja realmente excluir este evento?", "")
      .subscribe((retorno) => {
        if (retorno.result == "sim") {
          let dlg = this.alertas.alertarProcessamento("Processando exclusão...");
          this.wsEventos.excluir(evento.id)
            .subscribe((resposta) => {

              this.eventos = this.eventos.filter(x => x.id != evento.id);
              dlg.close();
            },
            (erro) => {
              dlg.close();
              this.alertas.alertarErro(erro);
            });
        }
      });
  }
}
