import { Component, OnInit, ViewChild } from '@angular/core';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { ActivatedRoute, Router } from '@angular/router';
import { DTOInscricaoCompleta, EnumApresentacaoAtividades, DTOSarau, DTOInscricaoSimplificada } from './objetos';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { WsManutencaoInscricoes } from '../webservices/wsManutencaoInscricoes';
import { DTOOficina, DTOSalaEstudo, DTODepartamento } from '../principal/objetos';

@Component({
    selector: 'tela-inscricao',
    templateUrl: './tela-inscricao.html',
    styleUrls: ['./tela-inscricao.scss']
})
export class TelaInscricao implements OnInit {

    dadosTela: DadosTela;

    @ViewChild("grupoValidacaoEssencial", { static: false })
    grupoValidacaoEssencial: DxValidationGroupComponent;

    @ViewChild("grupoValidacaoEspirita", { static: false })
    grupoValidacaoEspirita: DxValidationGroupComponent;

    inscricao: DTOInscricaoCompleta;

    constructor(public coordenacao: CoordenacaoCentral, private rotaAtual: ActivatedRoute, private navegadorUrl: Router, private wsInscricoes: WsManutencaoInscricoes) { }

    ngOnInit(): void {
        this.dadosTela = new DadosTela(this.coordenacao);

        let dlg = this.coordenacao.Alertas.alertarProcessamento("Carregando dados...");

        this.rotaAtual.params
            .subscribe(
                (parametrosUrl) => {
                    let idInscricao = parametrosUrl["idinscricao"];

                    this.wsInscricoes.obterInscricaoCompleta(idInscricao)
                        .subscribe(
                            (dadosInscricao) => {
                                dlg.close();
                                if (dadosInscricao != null) {
                                    this.inscricao = dadosInscricao;
                                    this.dadosTela.nome = this.inscricao.DadosPessoais.Nome;
                                    this.dadosTela.dataNascimento = this.inscricao.DadosPessoais.DataNascimento;
                                    this.dadosTela.email = this.inscricao.DadosPessoais.Email;
                                    this.dadosTela.descricaoEvento = this.inscricao.Evento.Nome;
                                    this.dadosTela.idadeMinima = this.inscricao.Evento.IdadeMinima;
                                    this.dadosTela.sexoEscolhido = this.coordenacao.Sexos[this.inscricao.DadosPessoais.Sexo];
                                    this.dadosTela.tipoInscricaoEscolhida = this.coordenacao.TiposInscricao[this.inscricao.DadosPessoais.TipoInscricao];
                                    this.dadosTela.cidade = this.inscricao.DadosPessoais.Cidade;
                                    this.dadosTela.uf = this.inscricao.DadosPessoais.Uf;
                                    this.dadosTela.ehVegetariano = this.inscricao.DadosPessoais.EhVegetariano;
                                    this.dadosTela.usaAdocanteDiariamente = this.inscricao.DadosPessoais.UsaAdocanteDiariamente;
                                    this.dadosTela.ehDiabetico = this.inscricao.DadosPessoais.EhDiabetico;
                                    this.dadosTela.carnesNaoCome = this.inscricao.DadosPessoais.CarnesNaoCome;
                                    this.dadosTela.alimentosAlergia = this.inscricao.DadosPessoais.AlimentosAlergia;
                                    this.dadosTela.medicamentosUsa = this.inscricao.DadosPessoais.MedicamentosUsa;
                                    this.dadosTela.centroEspirita = this.inscricao.CentroEspirita;
                                    this.dadosTela.tempoEspirita = this.inscricao.TempoEspirita;
                                    this.dadosTela.primeiroEncontro = this.inscricao.DadosPessoais.PrimeiroEncontro;
                                    this.dadosTela.nomeResponsavelCentro = this.inscricao.NomeResponsavelCentro;
                                    this.dadosTela.telefoneResponsavelCentro = this.inscricao.TelefoneResponsavelCentro;
                                    this.dadosTela.nomeResponsavelLegal = this.inscricao.NomeResponsavelLegal;
                                    this.dadosTela.telefoneResponsavelLegal = this.inscricao.TelefoneResponsavelLegal;

                                    this.dadosTela.dataMinimaNascimento = new Date(this.inscricao.Evento.PeriodoRealizacao.DataInicial);
                                    this.dadosTela.dataMinimaNascimento.setFullYear(this.dadosTela.dataMinimaNascimento.getFullYear() - this.inscricao.Evento.IdadeMinima);

                                    this.dadosTela.dataInicioEvento = new Date(this.inscricao.Evento.PeriodoRealizacao.DataInicial);

                                    if (this.inscricao.Oficina == null)
                                        this.dadosTela.oficinasEscolhidas = [];
                                    else if (this.inscricao.Oficina.Coordenador != null)
                                        this.dadosTela.oficinasEscolhidas = this.inscricao.Oficina.Coordenador
                                    else if (this.inscricao.Oficina.EscolhidasParticipante != null)
                                        this.dadosTela.oficinasEscolhidas = this.inscricao.Oficina.EscolhidasParticipante;

                                    if (this.inscricao.SalasEstudo == null)
                                        this.dadosTela.salasEscolhidas = [];
                                    else if (this.inscricao.SalasEstudo.Coordenador != null)
                                        this.dadosTela.salasEscolhidas = this.inscricao.SalasEstudo.Coordenador
                                    else if (this.inscricao.SalasEstudo.EscolhidasParticipante != null)
                                        this.dadosTela.salasEscolhidas = this.inscricao.SalasEstudo.EscolhidasParticipante;

                                    if (this.inscricao.Departamento == null)
                                        this.dadosTela.departamentoEscolhido = null;
                                    if (this.inscricao.Departamento.Coordenador != null)
                                        this.dadosTela.departamentoEscolhido = this.inscricao.Departamento.Coordenador
                                    else if (this.inscricao.Departamento.Participante != null)
                                        this.dadosTela.departamentoEscolhido = this.inscricao.Departamento.Participante;

                                    if (this.inscricao.Sarais == null)
                                        this.dadosTela.sarais = [];
                                    else
                                        this.dadosTela.sarais = this.inscricao.Sarais;
                                    this.dadosTela.inscricaoSimples = { Id: this.inscricao.Id, IdEvento: this.inscricao.Evento.Id, Nome: this.inscricao.DadosPessoais.Nome };
                                }
                                else
                                    this.voltar(idInscricao);
                            },
                            (erro) => {
                                dlg.close();
                                this.coordenacao.ProcessamentoErro.processar(erro);
                            }
                        );
                }
            );
    }

    clicarVoltar(): void {
        this.voltar(this.inscricao.Id);
    }

    private voltar(idInscricao: number) {
        this.coordenacao.AutorizacoesInscricao.remover(idInscricao);
        this.navegadorUrl.navigate(['pesquisar']);
    }

    clicarAtualizar(): void {
        let resultadoValidacaoEssencial = this.grupoValidacaoEssencial.instance.validate();
        if (resultadoValidacaoEssencial.isValid) {
            let mensagemAtencao = "";

            let resultadoValidacaoEspirita = this.grupoValidacaoEspirita.instance.validate();
            if (!resultadoValidacaoEspirita.isValid)
                mensagemAtencao = mensagemAtencao + "Há informações sobre a casa espírita " + (this.dadosTela.idade < 18 ? " ou do seu representante legal" : "") +
                    " que não foram informadas. Sem essas informações a secretaria do evento não poderá analisar e liberar a sua inscrição. Continuar mesmo assim?";

            if (mensagemAtencao != "") {
                this.coordenacao.Alertas.alertarConfirmacao("Precisamos da sua confirmação!!", mensagemAtencao)
                    .subscribe((botao) => {
                        if (botao.result == "sim")
                            this.atualizar();
                    });
            }
            else
                this.atualizar();

        }
        else
            this.coordenacao.Alertas.alertarAtencao("Nâo deu para continuar!!", "Ops, acho que alguns dados informados não estão legais!");
    }

    private atualizar(): void {
        let dlg = this.coordenacao.Alertas.alertarProcessamento("Atualizando dados...");

        this.wsInscricoes.atualizar(this.inscricao)
            .subscribe(
                (retorno) => {
                    this.voltar(this.inscricao.Id);
                },
                (erro) => {
                    dlg.close();
                    this.coordenacao.ProcessamentoErro.processar(erro);
                }
            );
    }
}

class DadosTela {
    nome: string;
    dataNascimento: Date;
    email: string;
    descricaoEvento: string;
    dataMinimaNascimento: Date;
    idadeMinima: number;
    sexoEscolhido: string;
    tipoInscricao: string;
    cidade: string;
    uf: string;
    ehVegetariano: boolean;
    usaAdocanteDiariamente: boolean;
    ehDiabetico: boolean;
    carnesNaoCome: string;
    alimentosAlergia: string;
    medicamentosUsa: string;
    centroEspirita: string;
    tempoEspirita: string;
    primeiroEncontro: boolean;
    nomeResponsavelCentro: string;
    telefoneResponsavelCentro: string;
    nomeResponsavelLegal: string;
    telefoneResponsavelLegal: string;
    dataInicioEvento: Date;

    formaEscolha: EnumApresentacaoAtividades;
    oficinasEscolhidas: DTOOficina | DTOOficina[];
    salasEscolhidas: DTOSalaEstudo | DTOSalaEstudo[];
    departamentoEscolhido: DTODepartamento;
    sarais: DTOSarau[];
    inscricaoSimples: DTOInscricaoSimplificada;

    constructor(private coordenacao: CoordenacaoCentral) { }

    get idade(): number {
        if (this.dataInicioEvento == null || this.dataNascimento == null)
            return 0;
        else {
            let idade = this.dataInicioEvento.getFullYear() - this.dataNascimento.getFullYear();
            let meses = this.dataInicioEvento.getMonth() - this.dataNascimento.getMonth();

            if (meses < 0 || (meses === 0 && this.dataInicioEvento.getDate() < this.dataNascimento.getDate()))
                idade--;

            return idade;
        }
    }

    get tipoInscricaoEscolhida(): string {
        return this.tipoInscricao;
    }

    set tipoInscricaoEscolhida(valor: string) {
        this.tipoInscricao = valor;
        if (valor == this.coordenacao.TiposInscricao[0])
            this.formaEscolha = EnumApresentacaoAtividades.ApenasParticipante
        else
            this.formaEscolha = EnumApresentacaoAtividades.PodeEscolher;
    }
}
