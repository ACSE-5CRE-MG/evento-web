import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { DTODadosCriarInscricao, DTODadosConfirmacao } from '../inscricao/objetos';

@Injectable()
export class WsInscricoes {

  public criar(idEvento: number, dadosIniciais: DTODadosCriarInscricao): Observable<DTODadosConfirmacao> {
    return new Observable<DTODadosConfirmacao>((x) => {

      let evento = new DTODadosConfirmacao();
      evento.IdInscricao = 1;
      evento.EnderecoEmail = dadosIniciais.Email;

      x.next(evento);
    });
  }  
}
