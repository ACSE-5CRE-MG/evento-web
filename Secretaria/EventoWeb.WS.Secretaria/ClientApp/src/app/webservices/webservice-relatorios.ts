import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs/Observable';

@Injectable()
export class WebServiceRelatorios extends WebServiceBase {

  constructor(http: HttpClient, public gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "relatorios/");
  }

  obterDivisaoSalas(idEvento: number): Observable<Blob> {
    return this.executarPutBlob('evento/' + idEvento.toString() + '/divisao-salas', null);
  };
}
