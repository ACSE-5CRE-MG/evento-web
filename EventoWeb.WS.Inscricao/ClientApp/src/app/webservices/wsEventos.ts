import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { DTOEventoListagem, Periodo } from '../principal/objetos';
import { ClienteWs } from './cliente-ws';
import { ConfiguracaoSistemaService } from '../configuracao-sistema-service';

@Injectable()
export class WsEventos {

    constructor(private clienteWs: ClienteWs) { }

    public listarEventosDisponiveisInscricao(): Observable<DTOEventoListagem[]> {

        this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'eventos/disponiveis';
        return this.clienteWs.executarGet("");
    }

    public obter(idEvento: number): Observable<DTOEventoListagem> {
        this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'eventos/' + idEvento.toString();
        return this.clienteWs.executarGet("");
    }
}
