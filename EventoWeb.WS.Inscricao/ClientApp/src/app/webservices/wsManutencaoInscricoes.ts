import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { DTOInscricaoCompleta, DTOSarau, DTOCrianca, DTOInscricaoAtualizacao } from '../inscricao/objetos';
import { ClienteWs } from './cliente-ws';
import { ConfiguracaoSistemaService } from '../configuracao-sistema-service';

@Injectable()
export class WsManutencaoInscricoes {

    constructor(private clienteWs: ClienteWs) { }

    obterInscricaoCompleta(idInscricao: number): Observable<DTOInscricaoCompleta> {

        this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'manutencaoinscricoes/obterInscricao/' + idInscricao.toString();
        return this.clienteWs.executarGet("");
    }

    atualizar(idInscricao: number, inscricao: DTOInscricaoAtualizacao): Observable<void> {

        this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'manutencaoinscricoes/atualizarInscricao/' + idInscricao.toString();
        return this.clienteWs.executarPut("", inscricao);
    }

    obterSarau(idEvento: number, codigo: string): Observable<DTOSarau> {
        this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'manutencaoinscricoes/evento/' + idEvento.toString() + '/obterSarau/' + codigo;
        return this.clienteWs.executarGet("");
    }

    obterCrianca(idEvento: number, codigo: string): Observable<DTOCrianca> {
        this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'manutencaoinscricoes/evento/' + idEvento.toString() + '/obterCrianca/' + codigo;
        return this.clienteWs.executarGet("");        
    }
}
