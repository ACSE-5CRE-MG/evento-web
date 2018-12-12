import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { WebServiceBase } from './webservice-base';
import { GestaoAutenticacao } from '../seguranca/gestao-autenticacao';
import { DTOEventoMinimo, DTOEventoCompleto, DTOEvento, DTOEventoIncluido } from '../evento/objetos';

@Injectable()
export class WebServiceEventos extends WebServiceBase {

  constructor(http: HttpClient, public gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "eventos/");
  }

  obterTodos(): Observable<DTOEventoMinimo[]> {
    return this.executarGet<DTOEventoMinimo[]>('obter-todos');
  };

  obterId(id: number): Observable<DTOEventoCompleto> {
    return this.executarGet<DTOEventoCompleto>('obter-id/' + id);
  };

  incluir(evento: DTOEvento): Observable<DTOEventoIncluido> {
    return this.executarPost('incluir/', evento);
  }

  atualizar(id: number, evento: DTOEvento): Observable<any> {
    return this.executarPut('atualizar/' + id, evento);
  }

  concluir(id: number): Observable<any> {
    return this.executarPut('concluir/' + id, null);
  }

  excluir(id: number): Observable<any> {
    return this.executarDelete('excluir/' + id);
  }
}
