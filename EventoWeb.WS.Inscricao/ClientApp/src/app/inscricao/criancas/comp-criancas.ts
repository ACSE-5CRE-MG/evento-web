import { Component, EventEmitter, Input, Output, ViewChild, Inject, Injectable } from '@angular/core';
import { DTOInscricaoSimplificada, DTOCrianca, EnumSexo, DTOInscricaoCompleta, DTOSarau } from '../objetos';
import { CoordenacaoCentral } from '../../componentes/central/coordenacao-central';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { Observable } from 'rxjs';
import { WsManutencaoInscricoes } from '../../webservices/wsManutencaoInscricoes';


@Component({
    selector: 'comp-criancas',
    templateUrl: './comp-criancas.html'
})
export class ComponenteCriancas {

    @Input()
    valor: DTOCrianca[];
    @Output()
    valorChange: EventEmitter<DTOCrianca[]> = new EventEmitter<DTOCrianca[]>();

    @Input()
    inscrito: DTOInscricaoCompleta;

    @Input()
    desabilitar: boolean;

    constructor(private coordenacao: CoordenacaoCentral, private DlgsCrianca: DialogosCrianca, private wsManInscricoes: WsManutencaoInscricoes) { }


    clicarCodigo(): void {
        this.DlgsCrianca.apresentarDlgCodigo()
            .subscribe(
                (codigo) => {
                    if (codigo != null) {

                        let dlg = this.coordenacao.Alertas.alertarProcessamento("Buscando criança pelo código...");

                        this.wsManInscricoes.obterCrianca(this.inscrito.Evento.Id, codigo.toUpperCase())
                            .subscribe(
                                (crianca) => {
                                    dlg.close();

                                    if (crianca != null) {
                                        crianca.DataNascimento = new Date(crianca.DataNascimento);
                                        crianca.Responsaveis.push({ Id: this.inscrito.Id, IdEvento: this.inscrito.Evento.Id, Nome: this.inscrito.DadosPessoais.Nome });
                                        this.valor.push(crianca);
                                        this.valorChange.emit(this.valor);
                                    }
                                    else
                                        this.coordenacao.Alertas.alertarInformacao("Criança não encontrada com o código informado, ou ela já tem os dois responsáveis definidos", "");
                                },
                                (erro) => {
                                    dlg.close();
                                    this.coordenacao.ProcessamentoErro.processar(erro);
                                }
                            );
                    }
                }
            );
    }

    clicarCriar(): void {
        this.DlgsCrianca.apresentarDlgForm({ crianca: null, inscricao: this.inscrito, desabilitar: this.desabilitar })
            .subscribe(
                (criancaCriada) => {
                    if (criancaCriada != null) {
                        criancaCriada.Responsaveis = [];
                        criancaCriada.Responsaveis.push({ Id: this.inscrito.Id, IdEvento: this.inscrito.Evento.Id, Nome: this.inscrito.DadosPessoais.Nome });

                        this.valor.push(criancaCriada);
                        this.valorChange.emit(this.valor);
                    }
                }
            );
    }

    editar(crianca: DTOCrianca): void {
        this.DlgsCrianca.apresentarDlgForm({ crianca: crianca, inscricao: this.inscrito, desabilitar: this.desabilitar })
            .subscribe(
                (criancaAlterada) => {
                    if (criancaAlterada != null) {
                        let indice = this.valor.findIndex(x => x.Id == criancaAlterada.Id);
                        if (indice != -1) {
                            this.valor[indice] = criancaAlterada;
                            this.valorChange.emit(this.valor);
                        }
                    }
                }
            );
    }

    excluir(crianca: DTOCrianca): void {

        this.coordenacao.Alertas.alertarConfirmacao("Deseja excluir este criança?", "")
            .subscribe(
                (botaoPressionado) => {
                    if (botaoPressionado.result == "sim") {
                        this.valor = this.valor.filter(x => x.Id != crianca.Id);
                        this.valorChange.emit(this.valor);
                    }
                });
    }

    gerarTextoResponsaveis(crianca: DTOCrianca): string {

        let pessoas: string = "";
        for (let inscricao of crianca.Responsaveis) {

            if (pessoas != "")
                pessoas = pessoas + ", ";

            if (inscricao.Id == this.inscrito.Id)
                pessoas = pessoas + "Você";
            else
                pessoas = pessoas + inscricao.Nome;
        }

        return pessoas;
    }
}

@Component({
    selector: 'dlg-crianca-codigo',
    templateUrl: './dlg-criancas-codigo.html'
})
export class DlgCriancaCodigo {

    codigo: string;

    @ViewChild("grupoValidacao", { static: false })
    grupoValidacao: DxValidationGroupComponent;

    constructor(private dialogRef: MatDialogRef<DlgCriancaCodigo>) { }

    clicarOK(): void {
        let validacao = this.grupoValidacao.instance.validate();
        if (validacao.isValid)
            this.dialogRef.close(this.codigo);
    }

    clicarCancelar(): void {
        this.dialogRef.close();
    }
}

@Component({
    selector: 'dlg-crianca-form',
    templateUrl: './dlg-criancas-form.html'
})
export class DlgCriancaFormulario {

    @ViewChild("grupoValidacao", { static: false })
    grupoValidacao: DxValidationGroupComponent;

    private crianca: DTOCrianca;

    dadosTela: DadosTela;
    tituloDlg: string;
    desabilitar: boolean;

    constructor(public coordenacao: CoordenacaoCentral,
        private dialogRef: MatDialogRef<DlgCriancaFormulario>,
        @Inject(MAT_DIALOG_DATA) public data: TransfDlgFormCrianca) {

        this.dadosTela = new DadosTela();
        this.tituloDlg = "Nova Inscrição";
        this.desabilitar = this.data.desabilitar;

        if (data.crianca == null) {
            this.crianca = new DTOCrianca();
            
            this.dadosTela.ehDiabetico = false;
            this.dadosTela.ehVegetariano = false;
            this.dadosTela.primeiroEncontro = false;
            this.dadosTela.usaAdocanteDiariamente = false;
        }
        else {
            this.crianca = data.crianca;

            this.dadosTela.alimentosAlergia = this.crianca.AlimentosAlergia;
            this.dadosTela.carnesNaoCome = this.crianca.CarnesNaoCome;
            this.dadosTela.dataNascimento = this.crianca.DataNascimento;
            this.dadosTela.ehDiabetico = this.crianca.EhDiabetico;
            this.dadosTela.ehVegetariano = this.crianca.EhVegetariano;
            this.dadosTela.medicamentosUsa = this.crianca.MedicamentosUsa;
            this.dadosTela.primeiroEncontro = this.crianca.PrimeiroEncontro;
            this.dadosTela.nome = this.crianca.Nome;
            this.dadosTela.sexoEscolhido = this.coordenacao.Sexos[this.crianca.Sexo];
            this.dadosTela.usaAdocanteDiariamente = this.crianca.UsaAdocanteDiariamente;
            this.dadosTela.email = this.crianca.Email;
            this.dadosTela.nomeCracha = this.crianca.NomeCracha;
            this.dadosTela.cidade = this.crianca.Cidade;
            this.dadosTela.uf = this.crianca.Uf;


            this.tituloDlg = "Alteração de Inscrição";
        }

        this.dadosTela.idadeMaxima = data.inscricao.Evento.IdadeMinima - 1;
        this.dadosTela.dataMaximaNascimento = new Date(data.inscricao.Evento.PeriodoRealizacao.DataInicial);
        this.dadosTela.dataMaximaNascimento.setFullYear(this.dadosTela.dataMaximaNascimento.getFullYear() - this.dadosTela.idadeMaxima);
    }

    clicarOK(): void {
        let validacao = this.grupoValidacao.instance.validate();
        if (validacao.isValid) {

            this.crianca.AlimentosAlergia = this.dadosTela.alimentosAlergia;
            this.crianca.CarnesNaoCome = this.dadosTela.carnesNaoCome;
            this.crianca.DataNascimento = this.dadosTela.dataNascimento;
            this.crianca.EhDiabetico = this.dadosTela.ehDiabetico;
            this.crianca.EhVegetariano = this.dadosTela.ehVegetariano;
            this.crianca.MedicamentosUsa = this.dadosTela.medicamentosUsa;
            this.crianca.PrimeiroEncontro = this.dadosTela.primeiroEncontro;
            this.crianca.Cidade = this.dadosTela.cidade;
            this.crianca.Uf = this.dadosTela.uf;
            this.crianca.Email = this.dadosTela.email;
            this.crianca.Nome = this.dadosTela.nome;
            this.crianca.Sexo = (this.dadosTela.sexoEscolhido == this.coordenacao.Sexos[0] ? EnumSexo.Masculino : EnumSexo.Feminino);
            this.crianca.UsaAdocanteDiariamente = this.dadosTela.usaAdocanteDiariamente;
            this.crianca.NomeCracha = this.dadosTela.nomeCracha;

            this.dialogRef.close(this.crianca);
        }
    }

    clicarCancelar(): void {
        this.dialogRef.close();
    }
}

class TransfDlgFormCrianca {
    crianca: DTOCrianca;
    inscricao: DTOInscricaoCompleta;
    desabilitar: boolean;
}

class DadosTela {
    nome: string;
    dataNascimento: Date;
    dataMaximaNascimento: Date;
    idadeMaxima: number;
    sexoEscolhido: string;
    ehVegetariano: boolean;
    usaAdocanteDiariamente: boolean;
    ehDiabetico: boolean;
    carnesNaoCome: string;
    alimentosAlergia: string;
    medicamentosUsa: string;
    cidade: string;
    uf: string;
    email: string;
    nomeCracha: string;
    primeiroEncontro: boolean;
    sarais: DTOSarau[];
}

@Injectable()
export class DialogosCrianca {
    constructor(private srvDialog: MatDialog) { }

    apresentarDlgCodigo(): Observable<string> {
        const dlg = this.srvDialog.open(DlgCriancaCodigo);
        return dlg.afterClosed();
    }

    apresentarDlgForm(dados: TransfDlgFormCrianca): Observable<DTOCrianca> {
        const dlg = this.srvDialog.open(DlgCriancaFormulario, { data: dados });
        return dlg.afterClosed();
    }
}
