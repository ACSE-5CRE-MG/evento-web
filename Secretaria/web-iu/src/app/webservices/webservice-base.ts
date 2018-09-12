import { Http, Response, Headers, RequestOptions, ResponseContentType } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { GestaoAutenticacao } from '../seguranca/gestao-autenticacao';

import { ConfiguracaoSistemaService } from '../configuracao-sistema-service'

export abstract class WebServiceBase {
  
  protected get webserviceURL(): string { return ConfiguracaoSistemaService.configuracao.urlBaseWs; }

  constructor(public http: Http, public gestorAutenticacao: GestaoAutenticacao,
    public nomeWebService: string) { }
    
  protected executarGet(parametros: string, retornoEhBinario: boolean = false): any {
    let opRequisicao = this.gerarCabecalho(retornoEhBinario);

    return this.http
      .get(this.webserviceURL + this.nomeWebService +
        this.gerarParametros(parametros), opRequisicao)
      .map(this.ExtrairDados)
      .catch(this.ProcessarErro);
  }

  protected executarPost(parametros: string, dados: any): any {
    let opRequisicao = this.gerarCabecalho();

    return this.http
      .post(this.webserviceURL + this.nomeWebService +
        this.gerarParametros(parametros),
        dados, opRequisicao)
      .map(this.ExtrairDados)
      .catch(this.ProcessarErro);
  }   

  protected executarPut(parametros: string, dados: any, retornoEhBinario: boolean = false): any {
    let opRequisicao = this.gerarCabecalho(retornoEhBinario);

    return this.http
      .put(this.webserviceURL + this.nomeWebService +
        this.gerarParametros(parametros),
        dados, opRequisicao)
      .map(this.ExtrairDados)
      .catch(this.ProcessarErro);
  }

  protected executarDelete(parametros: string): any {
    let opRequisicao = this.gerarCabecalho();

    return this.http
      .delete(this.webserviceURL + this.nomeWebService +
        this.gerarParametros(parametros),
        opRequisicao)
      .map(this.ExtrairDados)
      .catch(this.ProcessarErro);
  } 

  public gerarParametros(parametros: string): string {
    return (parametros != null && parametros.length > 0 ? parametros : '');
  }    

  public gerarCabecalho(ehBinario: boolean = false): RequestOptions {

    let opRequisicao = new RequestOptions();
    opRequisicao.headers = new Headers();
    opRequisicao.headers.append('Content-Type', 'application/json');
    opRequisicao.headers.append('Accept', 'application/json');
    console.log(this.gestorAutenticacao.dadosAutenticacao);
    if (this.gestorAutenticacao.autenticado)
      opRequisicao.headers.append('Authorization', 'Bearer ' + this.gestorAutenticacao.dadosAutenticacao.tokenApi);

    if (ehBinario)
      opRequisicao.responseType = ResponseContentType.Blob;

    return opRequisicao;
  }

  public ExtrairDados(res: Response) {
    try {
      if (res.headers.get("Content-Type").indexOf("application/json") != -1) {
        let body = res.json();
        return body || null;
      } else if (res.headers.get("Content-Type").indexOf("application/pdf") != -1 ||
        res.headers.get("Content-Type").indexOf("image/jpg") != -1 ||
        res.headers.get("Content-Type").indexOf("image/png") != -1) {
        return new Blob([res.blob()], { type: res.headers.get("Content-Type") });
      }
      else
        return null;
    } catch (erro) {
      return null;
    }
  }

  public ProcessarErro(error: Response | any) {
    // In a real world a..pp, we might use a remote logging infrastructure

    let errMsg: string;
    if (error instanceof Response) {
      const body = error.json() || '';

      if (body.LinkLogin != null) {
        errMsg = body.Excecao.Message
        window.location.href = body.LinkLogin;
      }
      else if (body.MessageDetail != null)
        errMsg = body.Message + "\n" + body.MessageDetail;
      else if (body.ExceptionMessage != null)
        errMsg = body.ExceptionMessage;
      else
        errMsg = body;

      //const err = body.error || JSON.stringify(body);
      //errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
    } else {
      errMsg = error.message ? error.message : error.toString();
    }

    return Observable.throw(errMsg);
  }
}
