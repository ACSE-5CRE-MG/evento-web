import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs/Observable';
import { EnumSituacaoInscricao, DTOBasicoInscricao, DTOInscricaoCompleta, DTOInscricaoAtualizacao } from "../inscricao/objetos";

@Injectable()
export class WebServiceInscricoes extends WebServiceBase {
    
    constructor(http: HttpClient, public gestorAutenticacao: GestaoAutenticacao) {
        super(http, gestorAutenticacao, "inscricoes/");
    }

    obterTodas(idEvento: number, situacao: EnumSituacaoInscricao): Observable<DTOBasicoInscricao[]> {
        return this.executarGet<DTOBasicoInscricao[]>('evento/' + idEvento.toString() + '/listarTodas?&situacao=' + situacao);
    };

    excluir(idEvento: number, idInscricao: number): Observable<any> {
        return this.executarDelete('evento/' + idEvento.toString() + '/excluir/' + idInscricao.toString());
    }

    obterInscricaoCompleta(idEvento: number, idInscricao: number): Observable<DTOInscricaoCompleta> {
        return this.executarGet<DTOInscricaoCompleta>('evento/' + idEvento.toString() + '/obter/' + idInscricao.toString());
    }

    aceitar(idEvento: number, idInscricao: number, atualizacao: DTOInscricaoAtualizacao): Observable<any> {
        return this.executarPut('evento/' + idEvento.toString() + '/aceitar/' + idInscricao.toString(), atualizacao);
    }

    rejeitar(idEvento: number, idInscricao: number): Observable<any> {
        return this.executarPut('evento/' + idEvento.toString() + '/rejeitar/' + idInscricao.toString(), null);
    }

    completar(idEvento: number, idInscricao: number, atualizacao: DTOInscricaoAtualizacao): Observable<any> {
        return this.executarPut('evento/' + idEvento.toString() + '/completar/' + idInscricao.toString(), atualizacao);
    }

    atualizar(idEvento: number, idInscricao: number, atualizacao: DTOInscricaoAtualizacao): Observable<any> {
        return this.executarPut('evento/' + idEvento.toString() + '/atualizar/' + idInscricao.toString(), atualizacao);
    }
}
