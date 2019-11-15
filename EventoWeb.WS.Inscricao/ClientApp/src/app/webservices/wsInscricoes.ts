import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { DTODadosCriarInscricao, DTODadosConfirmacao, DTOBasicoInscricao, DTOAcessoInscricao, DTOEnvioCodigoAcessoInscricao } from '../inscricao/objetos';
import { ClienteWs } from './cliente-ws';
import { ConfiguracaoSistemaService } from '../configuracao-sistema-service';

@Injectable()
export class WsInscricoes {

    constructor(private clienteWs: ClienteWs) { }

    public obterBasicoInscricao(idInscricao: number): Observable<DTOBasicoInscricao> {
        this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'inscricoes/basicoPorId/' + idInscricao.toString();
        return this.clienteWs.executarGet("");
    }

    public enviarCodigoAcesso(identificacao: string): Observable<DTOEnvioCodigoAcessoInscricao> {
        this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'inscricoes/enviarCodigo/' + identificacao.toString();
        return this.clienteWs.executarPut("", null);
    }

    public criar(idEvento: number, dadosIniciais: DTODadosCriarInscricao): Observable<DTODadosConfirmacao> {
        this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'inscricoes/criar/' + idEvento.toString();
        return this.clienteWs.executarPost("", dadosIniciais);
    }

    public validarAcessoInscricao(IdInscricao: number, codigo: string): Observable<DTOAcessoInscricao> {
        this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'inscricoes/validarCodigo/' + IdInscricao.toString();
        return this.clienteWs.executarPut("", "'" + codigo + "'");
    }
}
