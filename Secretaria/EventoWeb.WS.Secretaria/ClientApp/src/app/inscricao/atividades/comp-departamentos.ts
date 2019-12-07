import { Component, Input, Output, EventEmitter } from '@angular/core';
import { EnumApresentacaoAtividades, DTOInscricaoDepartamento } from '../objetos';
import { DTODepartamento } from '../../departamentos/objetos';

@Component({
    selector: 'comp-departamentos',
    templateUrl: './comp-departamentos.html'
})
export class ComponenteDepartamentos {

    apresentacao: EnumApresentacaoAtividades;
    opcoes: string[] = ["Participante", "Coordenador"];
    _opcaoEscolhida: string;

    private mDepartamentoEscolhido: DTODepartamento;

    @Input()
    desabilitar: boolean;

    @Input()
    departamentos: DTODepartamento[];

    @Input()
    set valor(param: DTOInscricaoDepartamento) {
        if (param == null)
            this.mDepartamentoEscolhido = null;
        else if (param.Coordenador != null) {
            this.opcaoEscolhida = this.opcoes[1];
            this.mDepartamentoEscolhido = param.Coordenador;
        }
        else {
            this.opcaoEscolhida = this.opcoes[0];
            this.mDepartamentoEscolhido = param.Participante;
        }
    }

    @Output()
    valorChange: EventEmitter<DTOInscricaoDepartamento> = new EventEmitter<DTOInscricaoDepartamento>();

    @Input()
    set forma(valor: EnumApresentacaoAtividades) {

        this.apresentacao = valor;

        if (this.apresentacao == EnumApresentacaoAtividades.ApenasParticipante)
          this.opcaoEscolhida = this.opcoes[0];
    }

    set opcaoEscolhida(valor: string) {
        if (valor != this._opcaoEscolhida) {
            this._opcaoEscolhida = valor;
            this.departamentoEscolhido = null;
        }
    }

    get opcaoEscolhida(): string {
        return this._opcaoEscolhida;
    }

    set departamentoEscolhido(valor: DTODepartamento) {
        this.mDepartamentoEscolhido = valor;

        if (valor == null)
            this.valorChange.emit(null);
        else
            this.valorChange.emit({
                Coordenador: this.opcaoEscolhida == this.opcoes[1] ? valor : null,
                Participante: this.opcaoEscolhida == this.opcoes[0] ? valor : null
            });
    }

    get departamentoEscolhido(): DTODepartamento {
        return this.mDepartamentoEscolhido;
    }
}
