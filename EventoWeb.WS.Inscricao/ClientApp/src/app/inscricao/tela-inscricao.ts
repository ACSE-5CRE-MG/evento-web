import { Component, OnInit, ViewChild } from '@angular/core';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { ActivatedRoute, Router } from '@angular/router';
import { DTOInscricaoCompleta, EnumSituacaoInscricao, DTOInscricaoAtualizacao } from './objetos';
import { WsManutencaoInscricoes } from '../webservices/wsManutencaoInscricoes';
import { DTOEventoCompleto } from '../principal/objetos';
import { CompFormInscricao } from './comp-form-inscricao';
import { WsInscricoes } from '../webservices/wsInscricoes';
import { WsEventos } from '../webservices/wsEventos';

export abstract class ATelaInscricao {
  inscricao: DTOInscricaoAtualizacao;
  evento: DTOEventoCompleto;
  NaoEhIncompleta: boolean = false;

  @ViewChild("formInscricao", { static: false })
  formInscricao: CompFormInscricao;

  constructor(public coordenacao: CoordenacaoCentral, protected navegadorUrl: Router) { }

  clicarVoltar(): void {
    this.navegadorUrl.navigate(['']);
  }

  clicarAtualizar(): void {
    let resultado = this.formInscricao.gerarAtualizacaoInscricao();
    if (resultado.valido)
      this.processarAtualizacao(resultado.inscricaoAtualizar);
  }

  protected abstract processarAtualizacao(inscricao: DTOInscricaoAtualizacao);
}

@Component({
  selector: 'tela-inscricao-atualizacao',
  templateUrl: './tela-inscricao.html',
  styleUrls: ['./tela-inscricao.scss']
})
export class TelaInscricaoAtualizacao extends ATelaInscricao implements OnInit {

  inscricaoCompleta: DTOInscricaoCompleta;

  constructor(public coordenacao: CoordenacaoCentral, private rotaAtual: ActivatedRoute, protected navegadorUrl: Router,
    private wsInscricoes: WsManutencaoInscricoes) {
    super(coordenacao, navegadorUrl);
  }

  ngOnInit(): void {
    this.inscricao = new DTOInscricaoCompleta();
    this.evento = new DTOEventoCompleto();

    let dlg = this.coordenacao.Alertas.alertarProcessamento("Carregando dados...");

    this.rotaAtual.params
      .subscribe(
        (parametrosUrl) => {
          let idInscricao = parametrosUrl["idinscricao"];

          this.wsInscricoes.obterInscricaoCompleta(idInscricao)
            .subscribe(
              (dadosInscricao) => {
                dlg.close();
                if (dadosInscricao != null) {
                  this.inscricao = dadosInscricao;
                  this.inscricaoCompleta = dadosInscricao;
                  this.evento = dadosInscricao.Evento;
                  this.NaoEhIncompleta = this.inscricaoCompleta.Situacao != EnumSituacaoInscricao.Incompleta;
                }
                else
                  this.voltar(idInscricao);

              },
              (erro) => {
                dlg.close();
                this.coordenacao.ProcessamentoErro.processar(erro);
              }
            );
        }
      );
  }

  clicarVoltar(): void {
    this.voltar(this.inscricaoCompleta.Id);
  }

  private voltar(idInscricao: number) {
    this.coordenacao.AutorizacoesInscricao.remover(idInscricao);
    super.clicarVoltar();
  }

  protected processarAtualizacao(inscricao: DTOInscricaoAtualizacao) {

    let dlg = this.coordenacao.Alertas.alertarProcessamento("Atualizando inscrição...");

    this.wsInscricoes.atualizar(this.inscricaoCompleta.Id, inscricao)
      .subscribe(
        (retorno) => {
          dlg.close();
          this.voltar(this.inscricaoCompleta.Id);
        },
        (erro) => {
          dlg.close();
          this.coordenacao.ProcessamentoErro.processar(erro);
        }
      );
  }
}

@Component({
  selector: 'tela-inscricao-inclusao',
  templateUrl: './tela-inscricao.html',
  styleUrls: ['./tela-inscricao.scss']
})
export class TelaInscricaoInclusao extends ATelaInscricao implements OnInit {

  constructor(public coordenacao: CoordenacaoCentral, private rotaAtual: ActivatedRoute, protected navegadorUrl: Router,
    private wsInscricoes: WsInscricoes, private wsEventos: WsEventos) {
    super(coordenacao, navegadorUrl);
  }

  ngOnInit(): void {
    this.inscricao = new DTOInscricaoAtualizacao();
    this.evento = new DTOEventoCompleto();
    this.NaoEhIncompleta = false;

    let dlg = this.coordenacao.Alertas.alertarProcessamento("Carregando dados...");

    this.rotaAtual.params
      .subscribe(
        (parametrosUrl) => {
          let idEvento = parametrosUrl["idevento"];

          this.wsEventos.obterCompleto(idEvento)
            .subscribe(
              (evento) => {
                if (evento == null)
                  this.clicarVoltar();
                else
                  this.evento = evento;
              },
              (erro) => {
                dlg.close();
                this.coordenacao.ProcessamentoErro.processar(erro);
              }
            );
          
        }
      );
  }

  protected processarAtualizacao(inscricao: DTOInscricaoAtualizacao) {

    let dlg = this.coordenacao.Alertas.alertarProcessamento("Atualizando inscrição...");

    this.wsInscricoes.criar(this.evento.Id, null)
      .subscribe(
        (retorno) => {
          dlg.close();
          this.coordenacao.Alertas.alertarInformacao("Inscrição enviada com sucesso!!",
            "Agora é aguardar a análise da secretaria do evento para que a sua inscrição seja efetivada!");
          this.clicarVoltar();
        },
        (erro) => {
          dlg.close();
          this.coordenacao.ProcessamentoErro.processar(erro);
        }
      );
  }
}
