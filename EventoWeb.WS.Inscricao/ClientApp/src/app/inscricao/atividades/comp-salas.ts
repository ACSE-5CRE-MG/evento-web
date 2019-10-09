import { Component, Input, Output, EventEmitter } from '@angular/core';
import { DTOSalaEstudo, EnumModeloDivisaoSalasEstudo } from '../../principal/objetos';
import { EnumApresentacaoAtividades } from '../objetos';

@Component({
  selector: 'comp-salas',
  templateUrl: './comp-salas.html'
})
export class ComponenteSalas {

  apresentacao: EnumApresentacaoAtividades;
  opcoes: string[] = ["Participante", "Coordenador"];
  _opcaoEscolhida: string;

  @Input()
  salas: DTOSalaEstudo[] = [];

  @Input()
  configuracaoSala: EnumModeloDivisaoSalasEstudo;

  @Input()
  escolhido: DTOSalaEstudo | DTOSalaEstudo[];

  @Output()
  escolhidoChange: EventEmitter<DTOSalaEstudo | DTOSalaEstudo[]> = new EventEmitter<DTOSalaEstudo | DTOSalaEstudo[]>();

  @Input()
  set forma(valor: EnumApresentacaoAtividades) {

    this.apresentacao = valor;
    this._opcaoEscolhida = this.opcoes[0];
  }

  set opcaoEscolhida(valor: string) {
    this._opcaoEscolhida = valor;

    if (valor == this.opcoes[0])
      this.salasEscolhidas = [];
    else
      this.salasEscolhidas = null;
  }

  get opcaoEscolhida(): string {
    return this._opcaoEscolhida;
  }

  set salasEscolhidas(valor: DTOSalaEstudo | DTOSalaEstudo[]) {
    this.escolhido = valor;
    this.escolhidoChange.emit(this.escolhido);
  }

  get salasEscolhidas(): DTOSalaEstudo | DTOSalaEstudo[] {
    return this.escolhido;
  }
}

@Component({
  selector: 'comp-sala-participante-com-escolha',
  templateUrl: './comp-sala-participante-com-escolha.html',
  styles: ['.borda_total { border: solid 1px; margin: 5px }']
})
export class ComponenteSalasParticipanteComEscolha {

  private salasRecebidas: DTOSalaEstudo[];

  @Input()
  set salas(valor: DTOSalaEstudo[]) {
    this.salasRecebidas = valor;
    if (valor != null) {
      if (this.salasEscolhidas != null && this.salasEscolhidas.length > 0)
        this.salasDisponiveis = valor.filter(x => this.salasEscolhidas.findIndex(y => y.Id == x.Id) == -1);
      else
        this.salasDisponiveis = valor;
    }
    else
      this.salasDisponiveis = [];
  }

  @Input()
  set valor(salas: DTOSalaEstudo[]) {

    if (salas != null) {
      this.salasEscolhidas = salas;

      if (this.salasRecebidas != null && this.salasRecebidas.length > 0)
        this.salasDisponiveis = this.salasRecebidas.filter(x => this.salasEscolhidas.findIndex(y => y.Id == x.Id) == -1);
      else
        this.salasDisponiveis = [];
    }
    else
      this.salasEscolhidas = [];
  }

  @Output()
  valorChange: EventEmitter<DTOSalaEstudo[]> = new EventEmitter<DTOSalaEstudo[]>();

  salasEscolhidas: DTOSalaEstudo[] = [];
  salasDisponiveis: DTOSalaEstudo[] = [];
  salasDisponiveisSelecionadas: DTOSalaEstudo[] = [];
  salasEscolhidasSelecionadas: DTOSalaEstudo[] = [];
  situacaoSelecaoSalasEscolhidas: EnumSituacaoSelecaoItens = EnumSituacaoSelecaoItens.NenhumItem;

  clicarIncluir(): void {
    for (let item of this.salasDisponiveisSelecionadas) {
      this.salasEscolhidas.push(item);
    }

    this.salasDisponiveis = this.salasDisponiveis.filter(x => this.salasDisponiveisSelecionadas.findIndex(y => y.Id == x.Id) == -1);
    this.salasDisponiveisSelecionadas = [];

    this.valorChange.emit(this.salasEscolhidas);
  }

  clicarRemover(): void {
    for (let item of this.salasEscolhidasSelecionadas)
      this.salasDisponiveis.push(item);

    this.salasEscolhidas = this.salasEscolhidas.filter(x => this.salasEscolhidasSelecionadas.findIndex(y => y.Id == x.Id) == -1);
    this.salasEscolhidasSelecionadas = [];

    this.valorChange.emit(this.salasEscolhidas);
  }

  processarMudancasSelecaoSalasEscolhidas(evento: any): void {

    if (this.salasEscolhidasSelecionadas.length == 0)
      this.situacaoSelecaoSalasEscolhidas = EnumSituacaoSelecaoItens.NenhumItem;
    else if (this.salasEscolhidasSelecionadas.length == 1) {
      this.atualizarSituacaoSelecaoItem();
    }
    else
      this.situacaoSelecaoSalasEscolhidas = EnumSituacaoSelecaoItens.MaisDeUmItem;
  }

  clicarSubir(): void {
    let posicaoItemSubirah = this.salasEscolhidas.findIndex(x => x.Id == this.salasEscolhidasSelecionadas[0].Id);
    let valorPosicaoIrah = this.salasEscolhidas[posicaoItemSubirah - 1];
    let valorPosicaoItemSubirah = this.salasEscolhidas[posicaoItemSubirah];

    this.salasEscolhidas[posicaoItemSubirah - 1] = valorPosicaoItemSubirah;
    this.salasEscolhidas[posicaoItemSubirah] = valorPosicaoIrah;

    this.valorChange.emit(this.salasEscolhidas);

    this.atualizarSituacaoSelecaoItem();
  }

  clicarDescer(): void {
    let posicaoItemDescerah = this.salasEscolhidas.findIndex(x => x.Id == this.salasEscolhidasSelecionadas[0].Id);
    let valorPosicaoIrah = this.salasEscolhidas[posicaoItemDescerah + 1];
    let valorPosicaoItemDescerah = this.salasEscolhidas[posicaoItemDescerah];

    this.salasEscolhidas[posicaoItemDescerah + 1] = valorPosicaoItemDescerah;
    this.salasEscolhidas[posicaoItemDescerah] = valorPosicaoIrah;

    this.valorChange.emit(this.salasEscolhidas);

    this.atualizarSituacaoSelecaoItem();
  }

  private atualizarSituacaoSelecaoItem(): void {
    let posicao = this.salasEscolhidas.findIndex(x => x.Id == this.salasEscolhidasSelecionadas[0].Id);
    if (posicao == 0)
      this.situacaoSelecaoSalasEscolhidas = EnumSituacaoSelecaoItens.Primeiro;
    else if (posicao == this.salasEscolhidas.length - 1)
      this.situacaoSelecaoSalasEscolhidas = EnumSituacaoSelecaoItens.Ultimo;
    else
      this.situacaoSelecaoSalasEscolhidas = EnumSituacaoSelecaoItens.EntrePrimeiroUltimo;
  }
}

enum EnumSituacaoSelecaoItens {
  Primeiro,
  Ultimo,
  EntrePrimeiroUltimo,
  MaisDeUmItem,
  NenhumItem
}

@Component({
  selector: 'comp-sala-participante-sem-escolha',
  templateUrl: './comp-sala-participante-sem-escolha.html'
})
export class ComponenteSalasParticipanteSemEscolha {
}

@Component({
  selector: 'comp-sala-coordenador',
  templateUrl: './comp-sala-coordenador.html'
})
export class ComponenteSalaCoordenador {

  @Input()
  salas: DTOSalaEstudo[] = [];

  @Input()
  valor: DTOSalaEstudo;

  @Output()
  valorChange: EventEmitter<DTOSalaEstudo> = new EventEmitter<DTOSalaEstudo>();

  set salaCoordena(valor: DTOSalaEstudo) {
    this.valor = valor;
    this.valorChange.emit(this.valor);
  }

  get salaCoordena(): DTOSalaEstudo {
    return this.valor;
  }
}
