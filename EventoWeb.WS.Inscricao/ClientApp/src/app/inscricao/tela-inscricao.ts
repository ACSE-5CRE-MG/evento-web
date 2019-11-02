import { Component, OnInit, ViewChild } from '@angular/core';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { ActivatedRoute, Router } from '@angular/router';
import {
    DTOInscricaoCompleta, EnumApresentacaoAtividades, DTOSarau, DTOInscricaoSimplificada,
    DTOCrianca, DTOPagamento, DTOInscricaoAtualizacao, EnumSexo, DTOInscricaoDadosPessoais, EnumTipoInscricao,
    DTOInscricaoOficina, DTOInscricaoSalaEstudo, DTOInscricaoDepartamento
} from './objetos';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { WsManutencaoInscricoes } from '../webservices/wsManutencaoInscricoes';
import { EnumModeloDivisaoSalasEstudo } from '../principal/objetos';

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
                                    this.dadosTela.tipoInscricaoEscolhida = this.coordenacao.TiposInscricao[this.inscricao.TipoInscricao];
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
                                    this.dadosTela.primeiroEncontro = this.inscricao.PrimeiroEncontro;
                                    this.dadosTela.nomeResponsavelCentro = this.inscricao.NomeResponsavelCentro;
                                    this.dadosTela.telefoneResponsavelCentro = this.inscricao.TelefoneResponsavelCentro;
                                    this.dadosTela.nomeResponsavelLegal = this.inscricao.NomeResponsavelLegal;
                                    this.dadosTela.telefoneResponsavelLegal = this.inscricao.TelefoneResponsavelLegal;

                                    this.dadosTela.dataMinimaNascimento = new Date(this.inscricao.Evento.PeriodoRealizacao.DataInicial);
                                    this.dadosTela.dataMinimaNascimento.setFullYear(this.dadosTela.dataMinimaNascimento.getFullYear() - this.inscricao.Evento.IdadeMinima);

                                    this.dadosTela.dataInicioEvento = new Date(this.inscricao.Evento.PeriodoRealizacao.DataInicial);
                                    this.dadosTela.oficinasEscolhidas = this.inscricao.Oficina;
                                    this.dadosTela.salasEscolhidas = this.inscricao.SalasEstudo;
                                    this.dadosTela.departamentoEscolhido = this.inscricao.Departamento;
                                    this.dadosTela.sarais = this.inscricao.Sarais;
                                    this.dadosTela.criancas = this.inscricao.Criancas;
                                    this.dadosTela.pagamento = this.inscricao.Pagamento;
                                    this.dadosTela.observacoes = this.inscricao.Observacoes;

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

    public contarCriancasPrimeiroResponsavel(): number {
        if (this.dadosTela.criancas == null)
            return 0;
        else
            return this.dadosTela.criancas.filter(x => x.Responsaveis[0] != null && x.Responsaveis[0].Id == this.inscricao.Id).length;        
    }

    private atualizar(): void {

        let dadosPessoaisValidos = this.grupoValidacaoEssencial.instance.validate().isValid;
        let dadosEspiritasValidos = this.grupoValidacaoEspirita.instance.validate().isValid;

        if (!dadosPessoaisValidos || !dadosEspiritasValidos)
            this.coordenacao.Alertas.alertarAtencao("Há informações pessoais que precisam de seus cuidados.", "Sem essas informações não é possível enviar a inscrição.");
        else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
            this.inscricao.Evento.TemOficinas &&
            this.dadosTela.oficinasEscolhidas == null)
            this.coordenacao.Alertas.alertarAtencao("Você não escolheu as oficinas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
        else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
            this.inscricao.Evento.TemOficinas &&
            this.dadosTela.oficinasEscolhidas.EscolhidasParticipante.length != this.inscricao.Evento.Oficinas.length)
            this.coordenacao.Alertas.alertarAtencao("Você não escolheu todas as oficinas.", "Sem essa informação não é possível enviar a inscrição.");
        else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
            this.inscricao.Evento.CnfSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
            this.dadosTela.salasEscolhidas == null)
            this.coordenacao.Alertas.alertarAtencao("Você não escolheu as salas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
        else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
            this.inscricao.Evento.CnfSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
            this.dadosTela.salasEscolhidas.EscolhidasParticipante.length != this.inscricao.Evento.SalasEstudo.length)
            this.coordenacao.Alertas.alertarAtencao("Você não escolheu todas as salas.", "Sem essa informação não é possível enviar a inscrição.");
        else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
            this.inscricao.Evento.TemDepartamentos &&
            this.dadosTela.departamentoEscolhido == null)
            this.coordenacao.Alertas.alertarAtencao("Você não escolheu o departamento que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
        else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[1] &&
            this.inscricao.Evento.TemOficinas &&
            this.dadosTela.oficinasEscolhidas.EscolhidasParticipante != null &&
            this.dadosTela.oficinasEscolhidas.EscolhidasParticipante.length != this.inscricao.Evento.Oficinas.length)
            this.coordenacao.Alertas.alertarAtencao("Você não escolheu todas as oficinas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
        else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[1] &&
            this.inscricao.Evento.CnfSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
            this.dadosTela.salasEscolhidas.EscolhidasParticipante != null &&
            this.dadosTela.salasEscolhidas.EscolhidasParticipante.length != this.inscricao.Evento.SalasEstudo.length)
            this.coordenacao.Alertas.alertarAtencao("Você não escolheu todas as salas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
        else if (this.dadosTela.pagamento == null)
            this.coordenacao.Alertas.alertarAtencao("Você precisa informar o Pagamento.", "Sem essa informação não é possível enviar a inscrição.");
        else {
            let dlg = this.coordenacao.Alertas.alertarProcessamento("Atualizando dados...");

            let atualizacao = new DTOInscricaoAtualizacao();
            atualizacao.DadosPessoais = new DTOInscricaoDadosPessoais();
            atualizacao.DadosPessoais.DataNascimento = this.dadosTela.dataNascimento;
            atualizacao.DadosPessoais.Email = this.dadosTela.email;
            atualizacao.DadosPessoais.Nome = this.dadosTela.nome;
            atualizacao.DadosPessoais.Sexo = (this.dadosTela.sexoEscolhido[0] == this.dadosTela.sexoEscolhido ? EnumSexo.Feminino : EnumSexo.Masculino);
            atualizacao.DadosPessoais.AlimentosAlergia = this.dadosTela.alimentosAlergia;
            atualizacao.DadosPessoais.CarnesNaoCome = this.dadosTela.carnesNaoCome;
            atualizacao.DadosPessoais.Cidade = this.dadosTela.cidade;
            atualizacao.DadosPessoais.EhDiabetico = this.dadosTela.ehDiabetico;
            atualizacao.DadosPessoais.EhVegetariano = this.dadosTela.ehVegetariano;
            atualizacao.DadosPessoais.MedicamentosUsa = this.dadosTela.medicamentosUsa;
            atualizacao.PrimeiroEncontro = this.dadosTela.primeiroEncontro;
            atualizacao.TipoInscricao = (this.coordenacao.TiposInscricao[0] == this.dadosTela.tipoInscricao ? EnumTipoInscricao.Participante : EnumTipoInscricao.ParticipanteTrabalhador);
            atualizacao.DadosPessoais.Uf = this.dadosTela.uf;
            atualizacao.DadosPessoais.UsaAdocanteDiariamente = this.dadosTela.usaAdocanteDiariamente;

            atualizacao.Sarais = this.dadosTela.sarais;
            atualizacao.Criancas = this.dadosTela.criancas;
            atualizacao.Pagamento = this.dadosTela.pagamento;
            atualizacao.Observacoes = this.dadosTela.observacoes;

            this.wsInscricoes.atualizar(this.inscricao.Id, atualizacao)
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
    observacoes: string;

    formaEscolha: EnumApresentacaoAtividades;
    oficinasEscolhidas: DTOInscricaoOficina;
    salasEscolhidas: DTOInscricaoSalaEstudo;
    departamentoEscolhido: DTOInscricaoDepartamento;
    sarais: DTOSarau[];
    inscricaoSimples: DTOInscricaoSimplificada;

    criancas: DTOCrianca[];

    pagamento: DTOPagamento;

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
