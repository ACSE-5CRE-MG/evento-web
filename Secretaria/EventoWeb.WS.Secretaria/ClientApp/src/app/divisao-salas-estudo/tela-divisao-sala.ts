import { Component, OnInit } from '@angular/core';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ActivatedRoute } from '@angular/router';
import { WebServiceDivisaoSala } from '../webservices/webservice-divisao-salas';
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { DTOEventoCompleto } from '../evento/objetos';
import { DTODivisaoSalaEstudo } from './objetos';
import { DTOBasicoInscricao } from '../inscricao/objetos';
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';

@Component({
  selector: 'tela-divisao-sala',
  styleUrls: ['./tela-divisao-sala.scss'],
  templateUrl: './tela-divisao-sala.html'
})
export class TelaDivisaoSala implements OnInit {

  private evento: DTOEventoCompleto = null;
  divisoesSalas: DTODivisaoSalaEstudo[] = [];
  inscricoesNaoDistribuidas: DTODivisaoSalaEstudo = null;

  divisaoSelecionada: DTODivisaoSalaEstudo = null;
  
  constructor(public wsDivisao: WebServiceDivisaoSala, public mensageria: Alertas,
    public roteador: ActivatedRoute, private srvEventoSelecionado: ServicoEventoSelecionado)
  {
    this.inscricoesNaoDistribuidas = {
      Id: 0,
      Nome: "",
      Coordenadores: [],
      Participantes: []
    };
  }

  ngOnInit(): void {

    this.evento = this.srvEventoSelecionado.EventoSelecionado;
    this.clicarAtualizar();
  }

  clicarAtualizar(): void {

    let dlg = this.mensageria.alertarProcessamento("Buscando as divisões de salas existentes...");

    this.wsDivisao.obterTodas(this.evento.Id)
      .subscribe(
        divisoes => {
          this.processarRetornoDivisao(divisoes);
          dlg.close();
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        });
  }

  clicarDividir(): void {
    this.mensageria.alertarConfirmacao("Deseja realizar a divisão automática?", "Ao realizar isso, as divisões existentes serão perdidas!!")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Realizando divisão...");

            this.wsDivisao.realizarDivisaoAutomatica(this.evento.Id)
              .subscribe(
                divisoes => {
                  this.processarRetornoDivisao(divisoes);
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

  clicarRemover(): void {
    this.mensageria.alertarConfirmacao("Deseja excluir a divisão atual?", "Ao realizar isso, nenhuma sala terá participantes!!")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Excluindo divisão...");

            this.wsDivisao.removerTudo(this.evento.Id)
              .subscribe(
                divisoes => {
                  this.processarRetornoDivisao(divisoes);
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

  private processarRetornoDivisao(divisoes: DTODivisaoSalaEstudo[]) {

    this.inscricoesNaoDistribuidas = divisoes.find(x => x.Id == 0);

    this.divisoesSalas = divisoes.filter(x => x.Id != 0);

    if (this.divisoesSalas.length > 0)
      this.divisaoSelecionada = this.divisoesSalas[0];
    else
      this.divisaoSelecionada = null;
  }

  clicarImprimir(): void {
  }

  clicarSelecionarSala(divisao: DTODivisaoSalaEstudo): void {
    this.divisaoSelecionada = divisao;
  }

  calcularIdade(inscricao: DTOBasicoInscricao): number {

    let dataAtual = new Date(this.evento.PeriodoRealizacao.DataInicial);
    let dataNascimento = new Date(inscricao.DataNascimento);

    let idade = (dataAtual.getFullYear() - dataNascimento.getFullYear() - 1) +
      (((dataAtual.getMonth() > dataNascimento.getMonth()) ||
        ((dataAtual.getMonth() == dataNascimento.getMonth()) &&
          (dataAtual.getDate() >= dataNascimento.getDate()))) ? 1 : 0);

    return idade;
  }
}
