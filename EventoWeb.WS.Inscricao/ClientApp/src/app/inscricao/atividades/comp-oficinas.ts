import { Component, ViewChild, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { DTOOficina } from '../../principal/objetos';
import { EnumApresentacaoAtividades } from '../objetos';

@Component({
  selector: 'comp-oficinas',
  templateUrl: './comp-oficinas.html'
})
export class ComponenteOficinas {

  apresentacao: EnumApresentacaoAtividades;
  opcoes: string[] = ["Participante", "Coordenador"];
  _opcaoEscolhida: string;

  @Input()
  oficinas: DTOOficina[] = [];

  @Input()
  escolhido: DTOOficina | DTOOficina[];

  @Output()
  escolhidoChange: EventEmitter<DTOOficina | DTOOficina[]> = new EventEmitter<DTOOficina | DTOOficina[]>();

  @Input()
  set forma(valor: EnumApresentacaoAtividades) {

    this.apresentacao = valor;
    this._opcaoEscolhida = this.opcoes[0];    
  }

  set opcaoEscolhida(valor: string) {
    this._opcaoEscolhida = valor;

    if (valor == this.opcoes[0])
      this.oficinasEscolhidas = [];
    else
      this.oficinasEscolhidas = null;
  }

  get opcaoEscolhida(): string {
    return this._opcaoEscolhida;
  }

  set oficinasEscolhidas(valor: DTOOficina | DTOOficina[]) {
    this.escolhido = valor;
    this.escolhidoChange.emit(this.escolhido);
  }

  get oficinasEscolhidas(): DTOOficina | DTOOficina[] {
    return this.escolhido;
  }
}

@Component({
  selector: 'comp-oficina-participante',
  templateUrl: './comp-oficina-participante.html',
  styles: ['.borda_total { border: solid 1px; margin: 5px }']
})
export class ComponenteOficinaParticipante {

  private oficinasRecebidas: DTOOficina[];

  @Input()
  set oficinas(valor: DTOOficina[]) {
    this.oficinasRecebidas = valor;
    if (valor != null) {
      if (this.oficinasEscolhidas != null && this.oficinasEscolhidas.length > 0)
        this.oficinasDisponiveis = valor.filter(x => this.oficinasEscolhidas.findIndex(y => y.Id == x.Id) == -1);
      else
        this.oficinasDisponiveis = valor;
    }
    else
      this.oficinasDisponiveis = [];
  }

  @Input()
  set valor(oficinas: DTOOficina[]) {

    if (oficinas != null) {
      this.oficinasEscolhidas = oficinas;

      if (this.oficinasRecebidas != null && this.oficinasRecebidas.length > 0)
        this.oficinasDisponiveis = this.oficinasRecebidas.filter(x => this.oficinasEscolhidas.findIndex(y => y.Id == x.Id) == -1);
      else
        this.oficinasDisponiveis = [];
    }
    else
      this.oficinasEscolhidas = [];
  }

  @Output()
  valorChange: EventEmitter<DTOOficina[]> = new EventEmitter<DTOOficina[]>();

  oficinasEscolhidas: DTOOficina[] = [];
  oficinasDisponiveis: DTOOficina[] = [];
  oficinasDisponiveisSelecionadas: DTOOficina[] = [];
  oficinasEscolhidasSelecionadas: DTOOficina[] = [];
  situacaoSelecaoOficinasEscolhidas: EnumSituacaoSelecaoItens = EnumSituacaoSelecaoItens.NenhumItem;

  clicarIncluir(): void {
    for (let item of this.oficinasDisponiveisSelecionadas) {
      this.oficinasEscolhidas.push(item);      
    }

    this.oficinasDisponiveis = this.oficinasDisponiveis.filter(x => this.oficinasDisponiveisSelecionadas.findIndex(y => y.Id == x.Id) == -1);
    this.oficinasDisponiveisSelecionadas = [];

    this.valorChange.emit(this.oficinasEscolhidas);
  }

  clicarRemover(): void {
    for (let item of this.oficinasEscolhidasSelecionadas)
      this.oficinasDisponiveis.push(item);

    this.oficinasEscolhidas = this.oficinasEscolhidas.filter(x => this.oficinasEscolhidasSelecionadas.findIndex(y => y.Id == x.Id) == -1);
    this.oficinasEscolhidasSelecionadas = [];

    this.valorChange.emit(this.oficinasEscolhidas);
  }

  processarMudancasSelecaoOficinasEscolhidas(evento: any): void {

    if (this.oficinasEscolhidasSelecionadas.length == 0)
      this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.NenhumItem;
    else if (this.oficinasEscolhidasSelecionadas.length == 1) {
      this.atualizarSituacaoSelecaoItem();
    }
    else
      this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.MaisDeUmItem;
  }

  clicarSubir(): void {
    let posicaoItemSubirah = this.oficinasEscolhidas.findIndex(x => x.Id == this.oficinasEscolhidasSelecionadas[0].Id);
    let valorPosicaoIrah = this.oficinasEscolhidas[posicaoItemSubirah - 1];
    let valorPosicaoItemSubirah = this.oficinasEscolhidas[posicaoItemSubirah];

    this.oficinasEscolhidas[posicaoItemSubirah - 1] = valorPosicaoItemSubirah;
    this.oficinasEscolhidas[posicaoItemSubirah] = valorPosicaoIrah;

    this.valorChange.emit(this.oficinasEscolhidas);

    this.atualizarSituacaoSelecaoItem();
  }

  clicarDescer(): void {
    let posicaoItemDescerah = this.oficinasEscolhidas.findIndex(x => x.Id == this.oficinasEscolhidasSelecionadas[0].Id);
    let valorPosicaoIrah = this.oficinasEscolhidas[posicaoItemDescerah + 1];
    let valorPosicaoItemDescerah = this.oficinasEscolhidas[posicaoItemDescerah];

    this.oficinasEscolhidas[posicaoItemDescerah + 1] = valorPosicaoItemDescerah;
    this.oficinasEscolhidas[posicaoItemDescerah] = valorPosicaoIrah;

    this.valorChange.emit(this.oficinasEscolhidas);

    this.atualizarSituacaoSelecaoItem();
  }

  private atualizarSituacaoSelecaoItem(): void {
    let posicao = this.oficinasEscolhidas.findIndex(x => x.Id == this.oficinasEscolhidasSelecionadas[0].Id);
    if (posicao == 0)
      this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.Primeiro;
    else if (posicao == this.oficinasEscolhidas.length - 1)
      this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.Ultimo;
    else
      this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.EntrePrimeiroUltimo;
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
  selector: 'comp-oficina-coordenador',
  templateUrl: './comp-oficina-coordenador.html'
})
export class ComponenteOficinaCoordenador {
  
  @Input()
  oficinas: DTOOficina[] = [];

  @Input()
  valor: DTOOficina;

  @Output()
  valorChange: EventEmitter<DTOOficina> = new EventEmitter<DTOOficina>();  

  set oficinaCoordena(valor: DTOOficina) {
    this.valor = valor;
    this.valorChange.emit(this.valor);
  }

  get oficinaCoordena(): DTOOficina {
    return this.valor;
  }
}