import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'tela-principal',
  templateUrl: './tela-principal.html',
  styleUrls: ['./tela-principal.scss']
})
export class TelaPrincipal implements OnInit {

  eventos: DTOEventoListagem[];

  ngOnInit(): void {

    this.eventos = [];

    let evento = new DTOEventoListagem();
    evento.Id = 1;
    evento.Logotipo = "";
    evento.Nome = "Evento 1";
    evento.PeriodoInscricao = new Periodo();
    evento.PeriodoInscricao.DataFinal = new Date();
    evento.PeriodoInscricao.DataInicial = new Date();
    evento.PeriodoRealizacao = new Periodo();
    evento.PeriodoRealizacao.DataFinal = new Date();
    evento.PeriodoRealizacao.DataInicial = new Date();

    this.eventos.push(evento);

    evento = new DTOEventoListagem();
    evento.Id = 2;
    evento.Logotipo = "";
    evento.Nome = "Evento 2";
    evento.PeriodoInscricao = new Periodo();
    evento.PeriodoInscricao.DataFinal = new Date();
    evento.PeriodoInscricao.DataInicial = new Date();
    evento.PeriodoRealizacao = new Periodo();
    evento.PeriodoRealizacao.DataFinal = new Date();
    evento.PeriodoRealizacao.DataInicial = new Date();
    this.eventos.push(evento);
  }

  public obterImagem(evento: DTOEventoListagem): string {
    if (evento.Logotipo == null || evento.Logotipo.trim().length == 0)
      return 'assets/semimagem.jpg';
    else
      return evento.Logotipo;
  }
}

export class DTOEventoListagem {
  public Id: number;
  public PeriodoInscricao: Periodo;
  public PeriodoRealizacao: Periodo;
  public Nome: string;
  public Logotipo: string;
}

export class Periodo {
  public DataInicial: Date;
  public DataFinal: Date;
}
