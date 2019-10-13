import { Component, Input, Output, EventEmitter } from '@angular/core';
import { EnumApresentacaoAtividades } from '../objetos';
import { DTODepartamento } from '../../principal/objetos';

@Component({
    selector: 'comp-departamentos',
    templateUrl: './comp-departamentos.html'
})
export class ComponenteDepartamentos {

    apresentacao: EnumApresentacaoAtividades;
    opcoes: string[] = ["Participante", "Coordenador"];
    _opcaoEscolhida: string;

    @Input()
    departamentos: DTODepartamento[] = [];

    @Input()
    escolhido: DTODepartamento;

    @Output()
    escolhidoChange: EventEmitter<DTODepartamento> = new EventEmitter<DTODepartamento>();

    @Input()
    set forma(valor: EnumApresentacaoAtividades) {

        this.apresentacao = valor;
        this._opcaoEscolhida = this.opcoes[0];
    }

    set opcaoEscolhida(valor: string) {
        this._opcaoEscolhida = valor;

        this.departamentoEscolhido = null;            
    }

    get opcaoEscolhida(): string {
        return this._opcaoEscolhida;
    }

    set departamentoEscolhido(valor: DTODepartamento) {
        this.escolhido = valor;
        this.escolhidoChange.emit(this.escolhido);
    }

    get departamentoEscolhido(): DTODepartamento {
        return this.escolhido;
    }
}
