import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  DTOInscricaoCompleta, EnumApresentacaoAtividades, DTOSarau, DTOInscricaoSimplificada,
  DTOCrianca, DTOPagamento, DTOInscricaoAtualizacao, EnumSexo, DTOInscricaoDadosPessoais, EnumTipoInscricao,
  DTOInscricaoOficina, DTOInscricaoSalaEstudo, DTOInscricaoDepartamento, EnumSituacaoInscricao, EnumPagamento
} from './objetos';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { EnumModeloDivisaoSalasEstudo, DTOEventoCompletoInscricao } from '../evento/objetos';
import { WebServiceInscricoes } from '../webservices/webservice-inscricoes';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';


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

  NaoEhIncompleta: boolean = false;

  private m_IdEvento: number;

  constructor(private mensageria: Alertas, private rotaAtual: ActivatedRoute, private navegadorUrl: Router, private wsInscricoes: WebServiceInscricoes) { }

  ngOnInit(): void {
    this.dadosTela = new DadosTela();
    this.inscricao = new DTOInscricaoCompleta();
    this.inscricao.Evento = new DTOEventoCompletoInscricao();

    let dlg = this.mensageria.alertarProcessamento("Carregando dados...");

    this.rotaAtual.parent.params
      .subscribe(
        (parametrosUrlPai) => {
          this.m_IdEvento = parametrosUrlPai["id"];

          this.rotaAtual.params
            .subscribe(
              (parametrosUrl) => {
                let idInscricao = parametrosUrl["idInscricao"];

                this.wsInscricoes.obterInscricaoCompleta(this.m_IdEvento, idInscricao)
                  .subscribe(
                    (dadosInscricao) => {
                      dlg.close();
                      if (dadosInscricao != null) {
                        this.inscricao = dadosInscricao;
                        this.dadosTela.nome = this.inscricao.DadosPessoais.Nome;
                        this.dadosTela.dataNascimento = new Date(this.inscricao.DadosPessoais.DataNascimento);
                        this.dadosTela.email = this.inscricao.DadosPessoais.Email;
                        this.dadosTela.descricaoEvento = this.inscricao.Evento.Nome;
                        this.dadosTela.idadeMinima = this.inscricao.Evento.IdadeMinima;
                        this.dadosTela.sexoEscolhido = this.dadosTela.Sexos[this.inscricao.DadosPessoais.Sexo];
                        this.dadosTela.tipoInscricaoEscolhida = this.dadosTela.TiposInscricao[this.inscricao.TipoInscricao];
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
                        this.dadosTela.telefoneFixo = this.inscricao.DadosPessoais.TelefoneFixo;
                        this.dadosTela.celular = this.inscricao.DadosPessoais.Celular;
                        this.dadosTela.nomeCracha = this.inscricao.NomeCracha;

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

                        if (this.dadosTela.pagamento.ComprovantesBase64 != null)
                          this.dadosTela.pagamento.ComprovantesBase64 = this.dadosTela.pagamento.ComprovantesBase64.map(x => 'data:image/jpeg;base64,' + x);

                        this.dadosTela.inscricaoSimples = { Id: this.inscricao.Id, IdEvento: this.inscricao.Evento.Id, Nome: this.inscricao.DadosPessoais.Nome };

                        this.NaoEhIncompleta = this.inscricao.Situacao != EnumSituacaoInscricao.Incompleta;
                      }
                      else
                        this.clicarVoltar();
                    },
                    (erro) => {
                      dlg.close();
                      this.mensageria.alertarErro(erro);
                    }
                  );
              }
            );
        }
      );
  }

  clicarVoltar(): void {
    this.navegadorUrl.navigate(['evento/' + this.m_IdEvento.toString() + '/inscricoes']);
  }

  clicarAceitar(): void {

    if (this.validarDados()) {
      let atualizacao = this.criarObjetoAtualizacao();
      let dlg = this.mensageria.alertarProcessamento("Registrando aceitação...");

      this.wsInscricoes.aceitar(this.inscricao.Evento.Id, this.inscricao.Id, atualizacao)
        .subscribe(
          (retorno) => {
            dlg.close();
            this.clicarVoltar();
          },
          (erro) => {
            dlg.close();
            this.mensageria.alertarErro(erro);
          }
        );
    }
  }

  clicarAtualizar(): void {
    if (this.validarDados()) {
      let atualizacao = this.criarObjetoAtualizacao();
      let dlg = this.mensageria.alertarProcessamento("Atualizando...");

      this.wsInscricoes.atualizar(this.inscricao.Evento.Id, this.inscricao.Id, atualizacao)
        .subscribe(
          (retorno) => {
            dlg.close();
            this.clicarVoltar();
          },
          (erro) => {
            dlg.close();
            this.mensageria.alertarErro(erro);
          }
        );
    }
  }

  clicarCompletarAceitar(): void {
    if (this.validarDados()) {
      let atualizacao = this.criarObjetoAtualizacao();
      let dlg = this.mensageria.alertarProcessamento("Atualizando...");

      this.wsInscricoes.completar(this.inscricao.Evento.Id, this.inscricao.Id, atualizacao)
        .subscribe(
          (retorno) => {
            dlg.close();
            this.clicarVoltar();
          },
          (erro) => {
            dlg.close();
            this.mensageria.alertarErro(erro);
          }
        );
    }
  }

  private validarDados(): boolean {

    let ehValido = false;

    let dadosPessoaisValidos = this.grupoValidacaoEssencial.instance.validate().isValid;
    let dadosEspiritasValidos = this.grupoValidacaoEspirita.instance.validate().isValid;

    if (!dadosPessoaisValidos || !dadosEspiritasValidos)
      this.mensageria.alertarAtencao("Há informações pessoais que precisam de seus cuidados.", "Sem essas informações não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[0] &&
      this.inscricao.Evento.TemOficinas &&
      this.dadosTela.oficinasEscolhidas == null)
      this.mensageria.alertarAtencao("Você não escolheu as oficinas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[0] &&
      this.inscricao.Evento.TemOficinas &&
      this.dadosTela.oficinasEscolhidas.EscolhidasParticipante.length != this.inscricao.Evento.Oficinas.length)
      this.mensageria.alertarAtencao("Você não escolheu todas as oficinas.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[0] &&
      this.inscricao.Evento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
      this.dadosTela.salasEscolhidas == null)
      this.mensageria.alertarAtencao("Você não escolheu as salas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[0] &&
      this.inscricao.Evento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
      this.dadosTela.salasEscolhidas.EscolhidasParticipante.length != this.inscricao.Evento.SalasEstudo.length)
      this.mensageria.alertarAtencao("Você não escolheu todas as salas.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[0] &&
      this.inscricao.Evento.TemDepartamentalizacao &&
      this.dadosTela.departamentoEscolhido == null)
      this.mensageria.alertarAtencao("Você não escolheu o departamento que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[1] &&
      this.inscricao.Evento.TemOficinas &&
      this.dadosTela.oficinasEscolhidas != null &&
      this.dadosTela.oficinasEscolhidas.EscolhidasParticipante != null &&
      this.dadosTela.oficinasEscolhidas.EscolhidasParticipante.length != this.inscricao.Evento.Oficinas.length)
      this.mensageria.alertarAtencao("Você não escolheu todas as oficinas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[1] &&
      this.inscricao.Evento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
      this.dadosTela.salasEscolhidas != null &&
      this.dadosTela.salasEscolhidas.EscolhidasParticipante != null &&
      this.dadosTela.salasEscolhidas.EscolhidasParticipante.length != this.inscricao.Evento.SalasEstudo.length)
      this.mensageria.alertarAtencao("Você não escolheu todas as salas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.pagamento == null || this.dadosTela.pagamento.Forma == null)
      this.mensageria.alertarAtencao("Você precisa informar o Pagamento.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.pagamento != null && this.dadosTela.pagamento.Forma == EnumPagamento.Comprovante &&
      (this.dadosTela.pagamento.ComprovantesBase64 == null || this.dadosTela.pagamento.ComprovantesBase64.length == 0))
      this.mensageria.alertarAtencao("Você precisa informar o(s) comprovante(s) de pagamento.", "Sem essa informação não é possível enviar a inscrição.");
    else
      ehValido = true;

    return ehValido;
  }

  private criarObjetoAtualizacao(): DTOInscricaoAtualizacao {

    let atualizacao = new DTOInscricaoAtualizacao();
    atualizacao.DadosPessoais = new DTOInscricaoDadosPessoais();
    atualizacao.DadosPessoais.DataNascimento = this.dadosTela.dataNascimento;
    atualizacao.DadosPessoais.Email = this.dadosTela.email;
    atualizacao.DadosPessoais.Nome = this.dadosTela.nome;
    atualizacao.DadosPessoais.Sexo = (this.dadosTela.sexoEscolhido == this.dadosTela.Sexos[0] ? EnumSexo.Masculino : EnumSexo.Feminino);
    atualizacao.DadosPessoais.AlimentosAlergia = this.dadosTela.alimentosAlergia;
    atualizacao.DadosPessoais.CarnesNaoCome = this.dadosTela.carnesNaoCome;
    atualizacao.DadosPessoais.Cidade = this.dadosTela.cidade;
    atualizacao.DadosPessoais.EhDiabetico = this.dadosTela.ehDiabetico;
    atualizacao.DadosPessoais.EhVegetariano = this.dadosTela.ehVegetariano;
    atualizacao.DadosPessoais.MedicamentosUsa = this.dadosTela.medicamentosUsa;
    atualizacao.DadosPessoais.TelefoneFixo = this.dadosTela.telefoneFixo;
    atualizacao.DadosPessoais.Celular = this.dadosTela.celular;
    atualizacao.PrimeiroEncontro = this.dadosTela.primeiroEncontro;
    atualizacao.TipoInscricao = (this.dadosTela.TiposInscricao[0] == this.dadosTela.tipoInscricao ? EnumTipoInscricao.Participante : EnumTipoInscricao.ParticipanteTrabalhador);
    atualizacao.DadosPessoais.Uf = this.dadosTela.uf;
    atualizacao.DadosPessoais.UsaAdocanteDiariamente = this.dadosTela.usaAdocanteDiariamente;
    atualizacao.NomeCracha = this.dadosTela.nomeCracha;
    atualizacao.CentroEspirita = this.dadosTela.centroEspirita;
    atualizacao.Departamento = this.dadosTela.departamentoEscolhido;
    atualizacao.NomeResponsavelCentro = this.dadosTela.nomeResponsavelCentro;
    atualizacao.NomeResponsavelLegal = this.dadosTela.nomeResponsavelLegal;
    atualizacao.Oficina = this.dadosTela.oficinasEscolhidas;
    atualizacao.SalasEstudo = this.dadosTela.salasEscolhidas;
    atualizacao.TelefoneResponsavelCentro = this.dadosTela.telefoneResponsavelCentro;
    atualizacao.TelefoneResponsavelLegal = this.dadosTela.telefoneResponsavelLegal;
    atualizacao.TempoEspirita = this.dadosTela.tempoEspirita;

    atualizacao.Sarais = this.dadosTela.sarais;
    atualizacao.Criancas = this.dadosTela.criancas;
    atualizacao.Observacoes = this.dadosTela.observacoes;

    atualizacao.Pagamento = new DTOPagamento();
    atualizacao.Pagamento.Forma = this.dadosTela.pagamento.Forma;
    atualizacao.Pagamento.Observacao = this.dadosTela.pagamento.Observacao;
    if (this.dadosTela.pagamento.ComprovantesBase64 != null)
      atualizacao.Pagamento.ComprovantesBase64 = this.dadosTela.pagamento.ComprovantesBase64.map(x => x.substring(x.indexOf(",") + 1));

    return atualizacao;
  }

  public clicarRejeitar(): void {

    this.mensageria.alertarConfirmacao("Deseja rejeitar esta Inscrição?", "")
      .subscribe(
        (botaoPressionado) => {
          if (botaoPressionado == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Registrando rejeição...");
            this.wsInscricoes.rejeitar(this.inscricao.Evento.Id, this.inscricao.Id)
              .subscribe(
                (retorno) => {
                  dlg.close();
                  this.clicarVoltar();
                },
                (erro) => {
                  dlg.close();
                  this.mensageria.alertarErro(erro);
                }
              );
          }
        });
  }

  public contarCriancasPrimeiroResponsavel(): number {
    if (this.dadosTela.criancas == null)
      return 0;
    else
      return this.dadosTela.criancas.filter(x => x.Responsaveis[0] != null && x.Responsaveis[0].Id == this.inscricao.Id).length;
  }
}

class DadosTela {

  Sexos: string[] = ["Masculino", "Feminino"];
  EstadosFederacao: string[] = ['AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA', 'MT', 'MS', 'MG', 'PA', 'PB', 'PR', 'PE', 'PI', 'RJ', 'RN', 'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO'];
  TiposInscricao: string[] = ['Participante', 'Participante/Trabalhador'];

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
  celular: string;
  telefoneFixo: string;
  nomeCracha: string;

  formaEscolha: EnumApresentacaoAtividades;
  oficinasEscolhidas: DTOInscricaoOficina;
  salasEscolhidas: DTOInscricaoSalaEstudo;
  departamentoEscolhido: DTOInscricaoDepartamento;
  sarais: DTOSarau[];
  inscricaoSimples: DTOInscricaoSimplificada;

  criancas: DTOCrianca[];

  pagamento: DTOPagamento;

  constructor() { }

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
    if (valor == this.TiposInscricao[0])
      this.formaEscolha = EnumApresentacaoAtividades.ApenasParticipante
    else
      this.formaEscolha = EnumApresentacaoAtividades.PodeEscolher;
  }
}
