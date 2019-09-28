import { Component, ViewChild, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { DTOOficina } from '../../principal/objetos';
import { DxValidationGroupComponent } from 'devextreme-angular';

@Component({
  selector: 'comp-oficinas',
  templateUrl: './comp-oficinas.html'
})
export class ComponenteOficinas {

  apresentacao: EnumApresentacaoOficina;
  opcoes: string[] = ["Participante", "Coordenador"];
  opcaoEscolhida: string;
  configParticipante: ConfiguracaoOficinaParticipante;
  configCoordenador: ConfiguracaoOficinaCoordenador;

  set resultadoCoordenador(valor: DTOOficina) {
    this.resultado.emit(valor);
  }

  set resultadoParticipante(valor: DTOOficina[]) {
    this.resultado.emit(valor);
  }

  @ViewChild("grupoValidacao", { static: false })
  grupoValidacao: DxValidationGroupComponent;

  @Input()
  set configuracao(cnf: ConfiguracaoOficina) {
    this.apresentacao = cnf.apresentacao;
    this.opcaoEscolhida = "";

    this.configParticipante = new ConfiguracaoOficinaParticipante();
    this.configParticipante.oficinasEvento = cnf.oficinasEvento;

    this.configCoordenador = new ConfiguracaoOficinaCoordenador();
    this.configCoordenador.oficinasEvento = cnf.oficinasEvento;
    this.configCoordenador.oficinaEscolhida = null;

    if (cnf.apresentacao == EnumApresentacaoOficina.ApenasParticipante) {
      this.opcaoEscolhida = this.opcoes[0];
      if (cnf.atribuidoInscricao == null)
        this.configParticipante.oficinasEscolhidas = [];
      else
        this.configParticipante.oficinasEscolhidas = <DTOOficina[]>cnf.atribuidoInscricao;
    }
    else if (cnf.atribuidoInscricao != null) {
      if (cnf.atribuidoInscricao instanceof DTOOficina) {
        this.opcaoEscolhida = this.opcoes[1];
        this.configCoordenador.oficinaEscolhida = <DTOOficina>cnf.atribuidoInscricao;
      }
      else {
        this.opcaoEscolhida = this.opcoes[0];
        this.configParticipante.oficinasEscolhidas = <DTOOficina[]>cnf.atribuidoInscricao;
      }
    }
  }

  @Output()
  resultado: EventEmitter<DTOOficina | DTOOficina[]> = new EventEmitter<DTOOficina | DTOOficina[]>();
}

export class ConfiguracaoOficina {
  apresentacao: EnumApresentacaoOficina;
  oficinasEvento: DTOOficina[];
  atribuidoInscricao: DTOOficina | DTOOficina[];
}

export enum EnumApresentacaoOficina {
  ApenasParticipante, PodeEscolher
}

@Component({
  selector: 'comp-oficina-participante',
  templateUrl: './comp-oficina-participante.html'
})
export class ComponenteOficinaParticipante {

  @ViewChild("grupoValidacao", { static: false })
  grupoValidacao: DxValidationGroupComponent;

  @Input()
  set configuracaoOficinas(cnf: ConfiguracaoOficinaParticipante) {
    this.oficinasDisponiveis = cnf.oficinasEvento.filter(x => cnf.oficinasEscolhidas.find(y => y.Id == x.Id) == null);
    this.oficinas = cnf.oficinasEscolhidas == null ? [] : cnf.oficinasEscolhidas;
  }

  @Output()
  resultados: EventEmitter<DTOOficina[]> = new EventEmitter<DTOOficina[]>();

  oficinas: DTOOficina[] = [];

  set oficinasEscolhidas(valor: DTOOficina[])
  {
    this.oficinas = valor;
    this.resultados.emit(this.oficinas);
  }

  get oficinasEscolhidas(): DTOOficina[] {
    return this.oficinas;
  }

  oficinasDisponiveis: DTOOficina[] = [];
  oficinasDisponiveisSelecionadas: DTOOficina[] = [];
  oficinasEscolhidasSelecionadas: DTOOficina[] = [];
  situacaoSelecaoOficinasEscolhidas: EnumSituacaoSelecaoItens = EnumSituacaoSelecaoItens.NenhumItem;

  clicarIncluir(): void {
    for (let item of this.oficinasDisponiveisSelecionadas) {
      this.oficinasEscolhidas.push(item);      
    }

    this.oficinasDisponiveis = this.oficinasDisponiveis.filter(x => this.oficinasDisponiveisSelecionadas.find(y => y.Id == x.Id) == null);
    this.oficinasDisponiveisSelecionadas = [];
  }

  clicarRemover(): void {
    for (let item of this.oficinasEscolhidasSelecionadas)
      this.oficinasDisponiveis.push(item);

    this.oficinasEscolhidas = this.oficinasEscolhidas.filter(x => this.oficinasEscolhidasSelecionadas.find(y => y.Id == x.Id) == null);
    this.oficinasEscolhidasSelecionadas = [];
  }

  processarMudancasSelecaoOficinasEscolhidas(evento: any): void {

    if (this.oficinasEscolhidasSelecionadas.length == 0)
      this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.NenhumItem;
    else if (this.oficinasEscolhidasSelecionadas.length == 1) {
      let posicao = this.oficinasEscolhidas.findIndex(x => x.Id == this.oficinasEscolhidasSelecionadas[0].Id);
      if (posicao == 0)
        this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.Primeiro;
      else if (posicao == this.oficinasEscolhidas.length - 1)
        this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.Ultimo;
      else
        this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.EntrePrimeiroUltimo;
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
  }

  clicarDescer(): void {
    let posicaoItemDescerah = this.oficinasEscolhidas.findIndex(x => x.Id == this.oficinasEscolhidasSelecionadas[0].Id);
    let valorPosicaoIrah = this.oficinasEscolhidas[posicaoItemDescerah + 1];
    let valorPosicaoItemDescerah = this.oficinasEscolhidas[posicaoItemDescerah];

    this.oficinasEscolhidas[posicaoItemDescerah + 1] = valorPosicaoItemDescerah;
    this.oficinasEscolhidas[posicaoItemDescerah] = valorPosicaoIrah;
  }
}

enum EnumSituacaoSelecaoItens {
  Primeiro,
  Ultimo,
  EntrePrimeiroUltimo,
  MaisDeUmItem,
  NenhumItem
}

export class ConfiguracaoOficinaParticipante {
  oficinasEvento: DTOOficina[];
  oficinasEscolhidas: DTOOficina[];
}

@Component({
  selector: 'comp-oficina-coordenador',
  templateUrl: './comp-oficina-coordenador.html'
})
export class ComponenteOficinaCoordenador {

  @ViewChild("grupoValidacao", { static: false })
  grupoValidacao: DxValidationGroupComponent;

  @Input()
  set configuracao(cnf: ConfiguracaoOficinaCoordenador) {
    this.oficinas = cnf.oficinasEvento;
    this.oficinaCoordena = this.oficinas.find(x => x.Id == cnf.oficinaEscolhida.Id);
  }

  oficinas: DTOOficina[] = [];

  @Input()
  oficinaCoordena: DTOOficina;

  @Output()
  oficinaCoordenaChange: EventEmitter<DTOOficina> = new EventEmitter<DTOOficina>();

  set oficina(valor: DTOOficina) {
    this.oficinaCoordena = valor;
    this.oficinaCoordenaChange.emit(this.oficina);
  }

  get oficina(): DTOOficina {
    return this.oficina;    
  }
}

export class ConfiguracaoOficinaCoordenador {
  oficinasEvento: DTOOficina[];
  oficinaEscolhida: DTOOficina;
}

export class ResultadoOficina {
  resultadoValido: boolean;
  escolha: DTOOficina | DTOOficina[];
}
