import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { DTODadosCriarInscricao, DTODadosConfirmacao, DTOBasicoInscricao, DTOAcessoInscricao, DTOEnvioCodigoAcessoInscricao, EnumResultadoEnvio, DTOInscricaoCompleta } from '../inscricao/objetos';

@Injectable()
export class WsInscricoes {

  public obterBasicoInscricao(idInscricao: number): Observable<DTOBasicoInscricao> {
    return new Observable<DTOBasicoInscricao>((x) => {

      let evento = new DTOBasicoInscricao();
      evento.IdInscricao = 1;
      evento.NomeInscrito = "João da Silva"
      evento.NomeEvento = "Evento Teste";
      evento.IdEvento = 1;
      evento.Email = "joao@uol.com.br"

      x.next(evento);
    });
  }

  public enviarCodigoAcesso(identificacao: string): Observable<DTOEnvioCodigoAcessoInscricao> {
    return new Observable<DTOEnvioCodigoAcessoInscricao>((x) => {

      let evento = new DTOEnvioCodigoAcessoInscricao();
      evento.IdInscricao = 1;
      evento.Resultado = EnumResultadoEnvio.InscricaoOK;

      x.next(evento);
    });
  }

  public criar(idEvento: number, dadosIniciais: DTODadosCriarInscricao): Observable<DTODadosConfirmacao> {
    return new Observable<DTODadosConfirmacao>((x) => {

      let evento = new DTODadosConfirmacao();
      evento.IdInscricao = 1;
      evento.EnderecoEmail = dadosIniciais.Email;

      x.next(evento);
    });
  }  

  public validarAcessoInscricao(IdInscricao: number, codigo: string): Observable<DTOAcessoInscricao> {
    return new Observable<DTOAcessoInscricao>((x) => {

      let evento = new DTOAcessoInscricao();
      evento.IdInscricao = 1;
      evento.Autorizacao = "Autorizacao";

      x.next(evento);
    });
  }

  obterInscricaoCompleta(idInscricao: number): Observable<DTOInscricaoCompleta> {
    return new Observable<DTOInscricaoCompleta>((x) => {

      let evento = new DTOInscricaoCompleta();

      x.next(evento);
    });
  }
}
