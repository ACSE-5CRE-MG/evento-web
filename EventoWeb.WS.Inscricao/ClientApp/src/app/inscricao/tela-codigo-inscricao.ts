import { Component, ViewChild, Input, OnInit } from '@angular/core';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { WsInscricoes } from '../webservices/wsInscricoes';
import { DTOAcessoInscricao } from './objetos';

@Component({
  selector: 'tela-codigo-inscricao',
  templateUrl: './tela-codigo-inscricao.html',
  styleUrls: ['./tela-codigo-inscricao.scss']
})
export class TelaCodigoInscricao implements OnInit {

  dadosTela: DadosTela;

  constructor(private rotaAtual: ActivatedRoute, private coordenacao: CoordenacaoCentral,
    private wsInscricoes: WsInscricoes, private navegadorUrl: Router) { }

  ngOnInit(): void {
    this.dadosTela = new DadosTela();
    this.dadosTela.codigo = "";
    this.dadosTela.dadosInscricao = new DTOAcessoInscricao();

    let dlg = this.coordenacao.Alertas.alertarProcessamento("Carregando dados...");

    this.rotaAtual.params
      .subscribe(
        (parametrosUrl) => {
          let idInscricao = parametrosUrl["idinscricao"];

          this.wsInscricoes.obterDadosAcesso(idInscricao)
            .subscribe(
              (dadosEvento) => {
                this.dadosTela.dadosInscricao = dadosEvento;

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

  @ViewChild("grupoValidacao", { static: false })
  grupoValidacao: DxValidationGroupComponent;

  public clicarContinuar(): void {

    this.grupoValidacao.instance.validate().isValid;
  }
}

class DadosTela {
  codigo: string;
  dadosInscricao: DTOAcessoInscricao;
}
