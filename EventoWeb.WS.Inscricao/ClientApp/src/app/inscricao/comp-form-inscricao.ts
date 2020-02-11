import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { ActivatedRoute, Router } from '@angular/router';
import {
  DTOInscricaoCompleta, EnumApresentacaoAtividades, DTOSarau, DTOInscricaoSimplificada,
  DTOCrianca, DTOPagamento, DTOInscricaoAtualizacao, EnumSexo, DTOInscricaoDadosPessoais, EnumTipoInscricao,
  DTOInscricaoOficina, DTOInscricaoSalaEstudo, DTOInscricaoDepartamento, EnumSituacaoInscricao, EnumPagamento
} from './objetos';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { WsManutencaoInscricoes } from '../webservices/wsManutencaoInscricoes';
import { EnumModeloDivisaoSalasEstudo, DTOEventoCompleto } from '../principal/objetos';

@Component({
  selector: 'comp-form-inscricao',
  templateUrl: './comp-form-inscricao.html',
  styleUrls: ['./comp-form-inscricao.scss']
})
export class CompFormInscricao implements OnInit {

  @Input()
  inscricao: DTOInscricaoAtualizacao;

  @Input()
  evento: DTOEventoCompleto;

  @Input()
  NaoEhIncompleta: boolean = false;

  dadosTela: DadosTela;

  @ViewChild("grupoValidacaoEssencial", { static: false })
  grupoValidacaoEssencial: DxValidationGroupComponent;

  @ViewChild("grupoValidacaoEspirita", { static: false })
  grupoValidacaoEspirita: DxValidationGroupComponent;

  constructor(public coordenacao: CoordenacaoCentral, private rotaAtual: ActivatedRoute, private navegadorUrl: Router, private wsInscricoes: WsManutencaoInscricoes) { }

  ngOnInit(): void {
    this.dadosTela = new DadosTela(this.coordenacao);
    this.dadosTela.nome = this.inscricao.DadosPessoais.Nome;
    this.dadosTela.dataNascimento = new Date(this.inscricao.DadosPessoais.DataNascimento);
    this.dadosTela.email = this.inscricao.DadosPessoais.Email;
    this.dadosTela.descricaoEvento = this.evento.Nome;
    this.dadosTela.idadeMinima = this.evento.IdadeMinima;
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
    this.dadosTela.telefoneFixo = this.inscricao.DadosPessoais.TelefoneFixo;
    this.dadosTela.celular = this.inscricao.DadosPessoais.Celular;
    this.dadosTela.nomeCracha = this.inscricao.NomeCracha;

    this.dadosTela.dataMinimaNascimento = new Date(this.evento.PeriodoRealizacao.DataInicial);
    this.dadosTela.dataMinimaNascimento.setFullYear(this.dadosTela.dataMinimaNascimento.getFullYear() - this.evento.IdadeMinima);

    this.dadosTela.dataInicioEvento = new Date(this.evento.PeriodoRealizacao.DataInicial);
    this.dadosTela.oficinasEscolhidas = this.inscricao.Oficina;
    this.dadosTela.salasEscolhidas = this.inscricao.SalasEstudo;
    this.dadosTela.departamentoEscolhido = this.inscricao.Departamento;
    this.dadosTela.sarais = this.inscricao.Sarais;
    this.dadosTela.criancas = this.inscricao.Criancas;
    this.dadosTela.pagamento = this.inscricao.Pagamento;
    this.dadosTela.observacoes = this.inscricao.Observacoes;

    if (this.dadosTela.pagamento.ComprovantesBase64 != null)
      this.dadosTela.pagamento.ComprovantesBase64 = this.dadosTela.pagamento.ComprovantesBase64.map(x => 'data:image/jpeg;base64,' + x);

    this.dadosTela.inscricaoSimples = { Id: 0, IdEvento: this.evento.Id, Nome: this.inscricao.DadosPessoais.Nome };
  }

  gerarAtualizacaoInscricao(): ResultadoAtualizacaoInscricao {

    let resultado = new ResultadoAtualizacaoInscricao();
    resultado.valido = false;
    resultado.inscricaoAtualizar = null;

    let dadosPessoaisValidos = this.grupoValidacaoEssencial.instance.validate().isValid;
    let dadosEspiritasValidos = this.grupoValidacaoEspirita.instance.validate().isValid;

    if (!dadosPessoaisValidos || !dadosEspiritasValidos)
      this.coordenacao.Alertas.alertarAtencao("Há informações pessoais que precisam de seus cuidados.", "Sem essas informações não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
      this.evento.TemOficinas &&
      this.dadosTela.oficinasEscolhidas == null)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu as oficinas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
      this.evento.TemOficinas &&
      this.dadosTela.oficinasEscolhidas.EscolhidasParticipante.length != this.evento.Oficinas.length)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu todas as oficinas.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
      this.evento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
      this.dadosTela.salasEscolhidas == null)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu as salas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
      this.evento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
      this.dadosTela.salasEscolhidas.EscolhidasParticipante.length != this.evento.SalasEstudo.length)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu todas as salas.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
      this.evento.TemDepartamentalizacao &&
      this.dadosTela.departamentoEscolhido == null)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu o departamento que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[1] &&
      this.evento.TemOficinas &&
      this.dadosTela.oficinasEscolhidas != null &&
      this.dadosTela.oficinasEscolhidas.EscolhidasParticipante != null &&
      this.dadosTela.oficinasEscolhidas.EscolhidasParticipante.length != this.evento.Oficinas.length)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu todas as oficinas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[1] &&
      this.evento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
      this.dadosTela.salasEscolhidas != null &&
      this.dadosTela.salasEscolhidas.EscolhidasParticipante != null &&
      this.dadosTela.salasEscolhidas.EscolhidasParticipante.length != this.evento.SalasEstudo.length)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu todas as salas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.pagamento == null || this.dadosTela.pagamento.Forma == null)
      this.coordenacao.Alertas.alertarAtencao("Você precisa informar o Pagamento.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.pagamento != null && this.dadosTela.pagamento.Forma == EnumPagamento.Comprovante &&
      (this.dadosTela.pagamento.ComprovantesBase64 == null || this.dadosTela.pagamento.ComprovantesBase64.length == 0))
      this.coordenacao.Alertas.alertarAtencao("Você precisa informar o(s) comprovante(s) de pagamento.", "Sem essa informação não é possível enviar a inscrição.");
    else {
      let atualizacao = new DTOInscricaoAtualizacao();
      atualizacao.DadosPessoais = new DTOInscricaoDadosPessoais();
      atualizacao.DadosPessoais.DataNascimento = this.dadosTela.dataNascimento;
      atualizacao.DadosPessoais.Email = this.dadosTela.email;
      atualizacao.DadosPessoais.Nome = this.dadosTela.nome;
      atualizacao.DadosPessoais.Sexo = (this.dadosTela.sexoEscolhido == this.coordenacao.Sexos[0] ? EnumSexo.Masculino : EnumSexo.Feminino);
      atualizacao.DadosPessoais.AlimentosAlergia = this.dadosTela.alimentosAlergia;
      atualizacao.DadosPessoais.CarnesNaoCome = this.dadosTela.carnesNaoCome;
      atualizacao.DadosPessoais.Cidade = this.dadosTela.cidade;
      atualizacao.DadosPessoais.EhDiabetico = this.dadosTela.ehDiabetico;
      atualizacao.DadosPessoais.EhVegetariano = this.dadosTela.ehVegetariano;
      atualizacao.DadosPessoais.MedicamentosUsa = this.dadosTela.medicamentosUsa;
      atualizacao.DadosPessoais.TelefoneFixo = this.dadosTela.telefoneFixo;
      atualizacao.DadosPessoais.Celular = this.dadosTela.celular;
      atualizacao.PrimeiroEncontro = this.dadosTela.primeiroEncontro;
      atualizacao.TipoInscricao = (this.coordenacao.TiposInscricao[0] == this.dadosTela.tipoInscricao ? EnumTipoInscricao.Participante : EnumTipoInscricao.ParticipanteTrabalhador);
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

      resultado.valido = true;
      resultado.inscricaoAtualizar = atualizacao;
    }

    return resultado;
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

export class ResultadoAtualizacaoInscricao {
  valido: boolean;
  inscricaoAtualizar: DTOInscricaoAtualizacao;
}