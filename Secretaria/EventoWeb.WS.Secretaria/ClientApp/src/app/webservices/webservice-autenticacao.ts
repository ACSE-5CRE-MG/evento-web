import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { WebServiceBase } from './webservice-base';
import { GestaoAutenticacao, Usuario } from '../seguranca/gestao-autenticacao';

@Injectable()
export class WebServiceAutenticacao extends WebServiceBase {

  constructor(http: HttpClient, public gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "autenticacao/");
  }

  autenticar(token: string): Observable<Usuario> {
    return this.executarPost('autenticar', token);
  };

  autenticarSemFacebook(emailFacebook: string, token: string): Observable<Usuario> {
    return this.executarPost('autenticarSemFacebook/?emailFacebook=' + emailFacebook + '&token=' + token, null);
  };

  desautenticar(): Observable<any> {
    return this.executarDelete('desautenticar/');
  }
}
