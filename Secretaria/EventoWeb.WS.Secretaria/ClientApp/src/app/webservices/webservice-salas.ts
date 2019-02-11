import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs/Observable';
import { DTOSalaEstudo } from "../sala-estudo/objetos";
import { DTOId } from "../evento/objetos";

@Injectable()
export class WebServiceSala extends WebServiceBase {

  constructor(http: HttpClient, public gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "sala/");
  }

  obterTodas(idEvento: number): Observable<DTOSalaEstudo[]> {
    return this.executarGet<DTOSalaEstudo[]>('listarTodas?idEvento=' + idEvento);
  };

  obterId(id: number): Observable<DTOSalaEstudo> {
    return this.executarGet<DTOSalaEstudo>('obter/' + id);
  };

  incluir(idEvento: number, sala: DTOSalaEstudo): Observable<DTOId> {
    return this.executarPost('criar/' + idEvento, sala);
  }

  atualizar(id: number, sala: DTOSalaEstudo): Observable<any> {
    return this.executarPut('atualizar/' + id, sala);
  }

  excluir(id: number): Observable<any> {
    return this.executarDelete('excluir/' + id);
  }
}
