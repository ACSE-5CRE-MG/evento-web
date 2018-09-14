import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { WebServiceBase } from './webservice-base';
import { GestaoAutenticacao } from '../seguranca/gestao-autenticacao';
import { DTOEventoMinimo, DTOEventoCompleto, DTOEvento } from '../evento/objetos';

@Injectable()
export class WebServiceEventos extends WebServiceBase {

  constructor(http: Http, public gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "eventos/");
  }

  obterTodos(): Observable<DTOEventoMinimo[]> {
    return this.executarGet('obter-todos');
  };

  obterId(id: number): Observable<DTOEventoCompleto> {
    return this.executarGet('obter-id/' + id);
  };

  incluir(evento: DTOEvento): Observable<any> {
    return this.executarPost('incluir/', evento);
  }

  atualizar(id: number, evento: DTOEvento): Observable<any> {
    return this.executarPut('atualizar/' + id, evento);
  }

  concluir(id: number): Observable<any> {
    return this.executarPut('concluir/' + id, null);
  }

  excluir(id: number, evento: DTOEvento): Observable<any> {
    return this.executarDelete('excluir/' + id);
  }
}
