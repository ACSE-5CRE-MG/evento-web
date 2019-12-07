import { Component, Injectable, OnInit } from "@angular/core";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";

import createNumberMask from 'text-mask-addons/dist/createNumberMask';
import { DTOEventoCompleto, EnumModeloDivisaoSalasEstudo, EnumPublicoEvangelizacao, Periodo } from "./objetos";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { WebServiceEventos } from "../webservices/webservice-eventos";
import { OperacoesImagem } from "../util/util-imagem";

@Component({
    selector: "dlg-form-evento",
    templateUrl: "./dlg-form-evento.html",
    styleUrls: ["./dlg-form-evento.scss"]
})
export class DlgFormEvento implements OnInit {

    titulo: string = "Inclusão de Evento";

    mascaraHora = {
        mask: [/\d/, /\d/, ':', /\d/, /\d/],
        guide: true
    };

    mascaraNumero = {
        mask: createNumberMask({
            prefix: '',
            suffix: '',
            thousandsSeparatorSymbol: '.',
            allowDecimal: false,
            allowNegative: false
        })
    };

    evento: DTOEventoCompleto;

    constructor(
        public dialogRef: MatDialogRef<DlgFormEvento>, public alertas: Alertas,
        public wsEventos: WebServiceEventos) { }

    ngOnInit(): void {
        this.evento = new DTOEventoCompleto();
        this.evento.PeriodoInscricao = new Periodo();
        this.evento.PeriodoRealizacao = new Periodo();
        this.evento.DataRegistro = new Date();
    }

    public set temSalaEstudo(valor: boolean) {
        if (valor) {
            this.evento.ConfiguracaoSalaEstudo = EnumModeloDivisaoSalasEstudo.PorIdadeCidade;
        }
        else
            this.evento.ConfiguracaoSalaEstudo = null;
    }

    public get temSalaEstudo(): boolean {
        return this.evento.ConfiguracaoSalaEstudo != null;
    }

    public set modeloDivisaoSalaEstudo(valor: EnumModeloDivisaoSalasEstudo) {
        if (this.evento.ConfiguracaoSalaEstudo != null)
            this.evento.ConfiguracaoSalaEstudo = valor;
    }

    public get modeloDivisaoSalEstudo(): EnumModeloDivisaoSalasEstudo {
        return (this.evento.ConfiguracaoSalaEstudo != null ? this.evento.ConfiguracaoSalaEstudo : null);
    }

    public set temEvangelizacao(valor: boolean) {
        if (valor) {
            this.evento.ConfiguracaoEvangelizacao = EnumPublicoEvangelizacao.Todos;
        }
        else
            this.evento.ConfiguracaoEvangelizacao = null;
    }

    public get temEvangelizacao(): boolean {
        return this.evento.ConfiguracaoEvangelizacao != null;
    }

    public set publicoEvangelizacao(valor: EnumPublicoEvangelizacao) {
        if (this.evento.ConfiguracaoEvangelizacao != null)
            this.evento.ConfiguracaoEvangelizacao = valor;
    }

    public get publicoEvangelizacao(): EnumPublicoEvangelizacao {
        return (this.evento.ConfiguracaoEvangelizacao != null ? this.evento.ConfiguracaoEvangelizacao : null);
    }

    public set temSarau(valor: boolean) {
        if (valor)
            this.evento.ConfiguracaoTempoSarauMin = 0;
        else
            this.evento.ConfiguracaoTempoSarauMin = null;
    }

    public get temSarau(): boolean {
        return this.evento.ConfiguracaoTempoSarauMin != null;
    }

    public set tempoDuracaoSarauMin(valor: number) {
        if (this.evento.ConfiguracaoTempoSarauMin != null)
            this.evento.ConfiguracaoTempoSarauMin = valor;
    }

    public get tempoDuracaoSarauMin(): number {
        return (this.evento.ConfiguracaoTempoSarauMin != null ? this.evento.ConfiguracaoTempoSarauMin : null);
    }

    processarArquivoEscolhido(arquivoImagem: any): void {
        let reader = new FileReader();
        reader.addEventListener('load', (event: any) => {
            this.evento.Logotipo = event.target.result;
        });
        reader.readAsDataURL(arquivoImagem.files[0]);
    }

    obterImagem(): string {
        return OperacoesImagem.obterImagemOuSemImagem(this.evento.Logotipo);
    }

    clicarLimparImagem(): void {
        this.evento.Logotipo = null;
    }

    clicarSalvar(): void {
        if (this.evento.Nome == null || this.evento.Nome.trim().length == 0)
            this.alertas.alertarAtencao("Você não informou o nome do evento", "");
        else if (this.evento.PeriodoRealizacao.DataInicial == null)
            this.alertas.alertarAtencao("Você não informou a data que inicia o evento", "");
        else if (this.evento.PeriodoRealizacao.DataFinal == null)
            this.alertas.alertarAtencao("Você não informou a data que termina o evento", "");
        else if (this.evento.PeriodoInscricao.DataInicial == null)
            this.alertas.alertarAtencao("Você não informou a data que inicia as inscrições", "");
        else if (this.evento.PeriodoInscricao.DataFinal == null)
            this.alertas.alertarAtencao("Você não informou a data que termina as inscrições", "");
        else if (this.evento.ConfiguracaoEvangelizacao != null && this.evento.ConfiguracaoEvangelizacao == null)
            this.alertas.alertarAtencao("Você precisa dizer qual o público da evangelização", "");
        else if (this.evento.ConfiguracaoSalaEstudo != null && this.evento.ConfiguracaoSalaEstudo == null)
            this.alertas.alertarAtencao("Você precisa dizer qual o modelo de divisão da sala de estudo", "");
        else if (this.evento.ConfiguracaoTempoSarauMin != null && (this.evento.ConfiguracaoTempoSarauMin <= 0))
            this.alertas.alertarAtencao("Você precisa dizer o tempo do duração. Esse tempo deve ser maior que zero", "");
        else {
            let dlg = this.alertas.alertarProcessamento("Salvando evento, aguarde...");
            this.wsEventos
                .incluir(this.evento)
                .subscribe(
                    (eventoIncluido) => {
                        dlg.close();
                        this.dialogRef.close(eventoIncluido);
                    },
                    (retornoErro) => {
                        dlg.close();
                        this.alertas.alertarErro(retornoErro);
                    });
        }
    }
}

@Injectable()
export class ServicoDlgFormEvento {

    constructor(public dialog: MatDialog) { }

    abrir() {
        const dialogRef = this.dialog.open(DlgFormEvento, {
            width: '100vw',
            height: '100vh',
            maxWidth: '100vw',
            maxHeight: '100vh'
        });

        return dialogRef.afterClosed();
    }
}
