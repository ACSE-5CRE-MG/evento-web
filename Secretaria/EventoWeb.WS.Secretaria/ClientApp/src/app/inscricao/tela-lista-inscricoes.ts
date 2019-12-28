import { Component, OnInit } from '@angular/core';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { Router, ActivatedRoute } from '@angular/router';
import { EnumSituacaoInscricao, DTOBasicoInscricao } from './objetos';
import { WebServiceInscricoes } from '../webservices/webservice-inscricoes';
import { Data } from '../seguranca/gestao-autenticacao';
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';

@Component({
  selector: 'tela-lista-inscricoes',
  templateUrl: './tela-lista-inscricoes.html',
  styleUrls: ["./tela-lista-inscricoes.scss"]
})
export class TelaListagemInscricoes implements OnInit {

  public filtros: string[] = ["Incompleta", "Pendente", "Aceita", "Rejeitada"];
  public inscricoes: DTOBasicoInscricao[] = [];
  private m_FiltroEscolhido: string = null;
  private m_IdEvento: number = null;

  constructor(public wsInscricoes: WebServiceInscricoes,
    public alertas: Alertas,
    public router: Router,
    public roteador: ActivatedRoute) { }

  set filtroEscolhido(valor: string) {
    if (valor != this.m_FiltroEscolhido) {
      this.m_FiltroEscolhido = valor;

      let dlg = this.alertas.alertarProcessamento("Pesquisando inscrições....");
      this.wsInscricoes.obterTodas(this.m_IdEvento, this.filtros.findIndex(x => x == this.m_FiltroEscolhido))
        .subscribe(
          inscricoesRetornadas => {
            dlg.close();
            this.inscricoes = inscricoesRetornadas.map(x => {
              x.DataNascimento = new Date(x.DataNascimento);
              return x;
            });
          },
          (erro) => {
            dlg.close();

            this.alertas.alertarErro(erro);
          }
        );
    }
  }

  get filtroEscolhido(): string {
    return this.m_FiltroEscolhido;
  }

  ngOnInit(): void {

    this.roteador.parent.params.subscribe(parametros => {
      this.m_IdEvento = +parametros["id"];
      this.filtroEscolhido = this.filtros[1];
    });
  }

  obterTextoSituacao(situacao: EnumSituacaoInscricao): string {
    switch (situacao) {
      case EnumSituacaoInscricao.Aceita:
        return "Aceita";
      case EnumSituacaoInscricao.Incompleta:
        return "Incompleta";
      case EnumSituacaoInscricao.Pendente:
        return "Pendente";
      case EnumSituacaoInscricao.Rejeitada:
        return "Rejeitada";
    }
  }

  editar(inscricao: DTOBasicoInscricao): void {

  }

  excluir(inscricao: DTOBasicoInscricao): void {
    this.alertas.alertarConfirmacao("Você deseja realmente excluir esta inscrição?", "")
      .subscribe((retorno) => {
        if (retorno == CaixaMensagemResposta.Sim) {
          let dlg = this.alertas.alertarProcessamento("Processando exclusão...");
          this.wsInscricoes.excluir(inscricao.IdEvento, inscricao.IdInscricao)
            .subscribe((resposta) => {

              this.inscricoes = this.inscricoes.filter(x => x.IdInscricao != inscricao.IdInscricao);
              dlg.close();
            },
              (erro) => {
                dlg.close();
                this.alertas.alertarErro(erro);
              });
        }
      });
  }
}
