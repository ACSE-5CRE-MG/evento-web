import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { DTODadosCriarInscricao, DTODadosConfirmacao, DTOAcessoInscricao } from '../inscricao/objetos';

@Injectable()
export class WsInscricoes {

  public obterDadosAcesso(idInscricao: number): Observable<DTOAcessoInscricao> {
    return new Observable<DTOAcessoInscricao>((x) => {

      let evento = new DTOAcessoInscricao();
      evento.IdInscricao = 1;
      evento.NomeInscrito = "João da Silva"
      evento.NomeEvento = "Evento Teste";
      evento.IdEvento = 1;
      evento.Email = "joao@uol.com.br"

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
}
