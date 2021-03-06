import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http'; 
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/switchMap';

import { GestaoAutenticacao } from '../seguranca/gestao-autenticacao';

import { ConfiguracaoSistemaService } from '../configuracao-sistema-service'
export abstract class WebServiceBase {
  
  protected get webserviceURL(): string { return ConfiguracaoSistemaService.configuracao.urlBaseWs; }

  constructor(public http: HttpClient, public gestorAutenticacao: GestaoAutenticacao,
    public nomeWebService: string) { }

  protected executarGet<T>(parametros: string): Observable<T> {
    let opRequisicao = this.gerarCabecalho();
    
    return this.http
      .get<T>(this.webserviceURL + this.nomeWebService +
          this.gerarParametros(parametros), { headers: opRequisicao })
      .catch(this.ProcessarErro);
  }

  protected executarPost(parametros: string, dados: any): any {
    let opRequisicao = this.gerarCabecalho();

    return this.http
      .post(this.webserviceURL + this.nomeWebService +
        this.gerarParametros(parametros),
      dados, { headers: opRequisicao })
      .catch(this.ProcessarErro);
  }   

  protected executarPut(parametros: string, dados: any, retornoEhBinario: boolean = false): any {
    let opRequisicao = this.gerarCabecalho();

    return this.http
      .put(this.webserviceURL + this.nomeWebService + this.gerarParametros(parametros),
        dados, { headers: opRequisicao })
      .catch(this.ProcessarErro);
  }

  protected executarPutBlob(parametros: string, dados: any): any {
    let opRequisicao = this.gerarCabecalho();

    return this.http
      .put(this.webserviceURL + this.nomeWebService + this.gerarParametros(parametros),
        dados,
        { headers: opRequisicao, responseType: 'blob' })
      .catch(this.ProcessarErro);
  }

  protected executarDelete(parametros: string): any {
    let opRequisicao = this.gerarCabecalho();

    return this.http
      .delete(this.webserviceURL + this.nomeWebService + this.gerarParametros(parametros),
        { headers: opRequisicao })
      //.map(this.ExtrairDados)
      .catch(this.ProcessarErro);
  } 

  public gerarParametros(parametros: string): string {
    return (parametros != null && parametros.length > 0 ? parametros : '');
  }    

  public gerarCabecalho(): HttpHeaders {

    let opRequisicao = new HttpHeaders();
    opRequisicao = opRequisicao.append('Content-Type', 'application/json');
    opRequisicao = opRequisicao.append('Accept', 'application/json');
    
    if (this.gestorAutenticacao.autenticado)
      opRequisicao = opRequisicao.append('Authorization', 'Bearer ' + this.gestorAutenticacao.dadosAutenticacao.TokenApi);

    /*if (ehBinario)
      opRequisicao.responseType = ResponseContentType.Blob;*/

    return opRequisicao;
  }

  public ProcessarErro(erro: HttpErrorResponse) {

    if (erro.error instanceof Blob && erro.error.type === "application/json") {
      let blobComoTextoNotificacao = new Observable<string>(notificador => {
        const leitorArquivo = new FileReader();
        leitorArquivo.onload = (e) => {
          let responseText = (<any>e.target).result;

          notificador.next(responseText);
          notificador.complete();
        }
        const errMsg = leitorArquivo.readAsText(erro.error, 'utf-8');
      });

      return blobComoTextoNotificacao
        .switchMap(errMsgJsonComoTexto => {
          return Observable.throw(JSON.parse(errMsgJsonComoTexto));
        });
    }
    else if (erro.error != null && !(erro.error instanceof Blob)) {
      return Observable.throw(erro.error);
    }
    else
      return Observable.throw(erro.message || "Erro no Servidor");
  }
}
