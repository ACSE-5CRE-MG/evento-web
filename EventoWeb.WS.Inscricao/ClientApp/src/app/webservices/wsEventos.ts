import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { DTOEventoListagem, Periodo } from '../principal/objetos';

@Injectable()
export class WsEventos {

  public listarEventosDisponiveisInscricao(): Observable<DTOEventoListagem[]> {
    return new Observable<DTOEventoListagem[]>((x) => {
      let eventos: DTOEventoListagem[] = [];

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

      eventos.push(evento);

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
      eventos.push(evento);

      x.next(eventos);
    });
  }

}
