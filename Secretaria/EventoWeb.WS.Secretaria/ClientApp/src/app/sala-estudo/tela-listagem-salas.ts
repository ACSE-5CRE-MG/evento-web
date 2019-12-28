import { Component, OnInit } from "@angular/core";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { ActivatedRoute } from "@angular/router";
import { WebServiceSala } from "../webservices/webservice-salas";
import { DTOSalaEstudo } from "./objetos";
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { DialogosSala } from './dlg-form-sala';


@Component({
  selector: 'tela-listagem-salas',
  styleUrls: ['./tela-listagem-salas.scss'],
  templateUrl: './tela-listagem-salas.html'
})
export class TelaListagemSalas implements OnInit {

  salaSelecionada: DTOSalaEstudo = null;
  salas: DTOSalaEstudo[] = [];

  private idEvento: number = 0;

  constructor(private wsSalas: WebServiceSala, private mensageria: Alertas, private roteador: ActivatedRoute, private dlgsSala: DialogosSala) { }

  ngOnInit(): void {
    this.roteador.parent.params.subscribe(parametros => {
      let dlg = this.mensageria.alertarProcessamento("Buscando salas...");

      this.idEvento = parametros["id"];

      this.wsSalas.obterTodas(this.idEvento)
        .subscribe(salas => {
          this.salas = salas;
          dlg.close();
        },
          erro => {
            dlg.close();
            this.mensageria.alertarErro(erro);
          });
    });
  }

  clicarIncluir(): void {
    this.dlgsSala.apresentarDlgFormDialogoInclusao(this.idEvento, false)
      .subscribe(
        (novaSala) => {
          if (novaSala != null)
            this.salas.push(novaSala);
        }
      );
  }

  clicarEditar(sala: DTOSalaEstudo): void {
    this.dlgsSala.apresentarDlgFormDialogoEdicao(this.idEvento, sala.Id, false)
      .subscribe(
        (salaAlterada) => {
          if (salaAlterada != null) {
            let indice = this.salas.findIndex(x => x.Id == salaAlterada.Id);
            if (indice != -1) {
              this.salas[indice] = salaAlterada;
            }
          }            
        }
      );
  }

  clicarExcluir(sala: DTOSalaEstudo): void {
    this.mensageria.alertarConfirmacao("Você quer excluir esta sala mesmo?", "")
      .subscribe(
        (resposta) => {
          if (resposta == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Processando exclusão de sala...");
            this.wsSalas.excluir(this.idEvento, sala.Id)
              .subscribe(
                exclusao => {
                  this.salas = this.salas.filter(x => x.Id != sala.Id);
                  dlg.close();
                },
                erro => {
                  dlg.close();
                  this.mensageria.alertarErro(erro);
                });
          }
        }
      );
  }
}
