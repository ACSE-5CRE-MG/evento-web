import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { WsEventos } from '../webservices/wsEventos';

@Component({
  selector: 'tela-criacao-inscricao',
  templateUrl: './tela-criacao-inscricao.html',
  styleUrls: ['./tela-criacao-inscricao.scss']
})
export class TelaCriacaoInscricao implements OnInit {

  idEvento: number;
  nomeEvento: string;
  eventoEncontrado: boolean;

  constructor(private rotaAtual: ActivatedRoute, public coordenacao: CoordenacaoCentral,
    private wsEventos: WsEventos, private navegadorUrl: Router) { }

  ngOnInit(): void {

    this.coordenacao.AutorizacoesInscricao.removerTudo();
    this.eventoEncontrado = false;
    this.nomeEvento = "";
    this.idEvento = 0;
    let dlg = this.coordenacao.Alertas.alertarProcessamento("Carregando dados...");

    this.rotaAtual.params
      .subscribe(
        (parametrosUrl) => {
          this.idEvento = parametrosUrl["idevento"];

          this.wsEventos.obter(this.idEvento)
            .subscribe(
              (dadosEvento) => {

                if (dadosEvento != null) {
                  this.nomeEvento = dadosEvento.Nome;
                  this.eventoEncontrado = true;
                }

                dlg.close();
              },
              (erro) => {
                dlg.close();
                this.coordenacao.ProcessamentoErro.processar(erro);
              }
            );
        }
      );
  }

  public clicarContinuar(): void {

    this.navegadorUrl.navigate(['evento/' + this.idEvento  + '/criar-inscricao']);    
  }
}
