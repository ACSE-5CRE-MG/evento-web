import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DTOPagamento } from '../objetos';

@Component({
    selector: 'comp-pagamento',
    templateUrl: './comp-pagamento.html'
})
export class ComponentePagamento {

    private mValor: DTOPagamento;

    @Input()
    set valor(param: DTOPagamento) {
        if (param == null)
            this.mValor = new DTOPagamento();
        else {
            this.mValor = param;
            this.opcaoEscolhida = this.opcoes[this.mValor.Forma];
        }
    }
    @Output()
    valorChange: EventEmitter<DTOPagamento> = new EventEmitter<DTOPagamento>();

    @Input()
    valorInscricao: number;

    @Input()
    valorInscCrianca: number;

    @Input()
    totalCriancas: number;

    opcoes: string[] = ["Enviar Comprovante", "Comprovante esta em outra inscrição", "Outros"];
    opcaoEscolhida: string;

    set comprovantes(param: any[]) {

        let arquivosValidos: File[] = [];
        if (param != null && param.length > 0)
            arquivosValidos = param.filter(x => x.type == "jpeg" || x.type == "jpg");

        if (arquivosValidos.length == 0) {
            this.mValor.ComprovantesBase64 = null;
            this.mValor.Observacao = "";
            this.valorChange.emit(null);
        }
        else {
            this.mValor.ComprovantesBase64 = [];
            this.valorChange.emit(this.mValor);
        }
    }

    get comprovantes(): any[] {
        if (this.mValor.ComprovantesBase64 == null)
            return [];
        else
            return this.mValor.ComprovantesBase64;
    }

    set observacoes(param: string) {

        if (this.opcaoEscolhida != this.opcoes[0] && (param == null || param.trim().length == 0)) {
            this.mValor.Observacao = "";
            this.valorChange.emit(null);
        }
        else {
            this.mValor.Observacao = param;
            this.valorChange.emit(this.mValor);
        }
    }

    get observacoes(): string {
        return this.mValor.Observacao;
    }
}
