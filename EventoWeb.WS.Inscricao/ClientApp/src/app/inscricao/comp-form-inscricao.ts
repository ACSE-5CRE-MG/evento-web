import { Component, ViewChild, Input } from '@angular/core';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { ActivatedRoute, Router } from '@angular/router';
import {
  EnumApresentacaoAtividades, DTOSarau, DTOInscricaoSimplificada,
  DTOPagamento, DTOInscricaoAtualizacao, EnumSexo, DTOInscricaoDadosPessoais, EnumTipoInscricao,
  DTOInscricaoOficina, DTOInscricaoSalaEstudo, DTOInscricaoDepartamento, EnumPagamento
} from './objetos';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { WsManutencaoInscricoes } from '../webservices/wsManutencaoInscricoes';
import { EnumModeloDivisaoSalasEstudo, DTOEventoCompleto } from '../principal/objetos';

@Component({
  selector: 'comp-form-inscricao',
  templateUrl: './comp-form-inscricao.html',
  styleUrls: ['./comp-form-inscricao.scss']
})
export class CompFormInscricao {  

  @Input()
  naoEhIncompleta: boolean = false;

  dadosTela: DadosTela;

  private mEvento: DTOEventoCompleto;
  private mInscricao: DTOInscricaoAtualizacao;

  @ViewChild("grupoValidacaoEssencial", { static: false })
  grupoValidacaoEssencial: DxValidationGroupComponent;

  @ViewChild("grupoValidacaoEspirita", { static: false })
  grupoValidacaoEspirita: DxValidationGroupComponent;

  constructor(public coordenacao: CoordenacaoCentral, private rotaAtual: ActivatedRoute, private navegadorUrl: Router, private wsInscricoes: WsManutencaoInscricoes) { }

  @Input()
  set inscricao(valor: DTOInscricaoAtualizacao) {

    this.mInscricao = valor;

    this.dadosTela = new DadosTela(this.coordenacao);
    this.dadosTela.nome = this.mInscricao.DadosPessoais.Nome;
    this.dadosTela.dataNascimento = this.mInscricao.DadosPessoais.DataNascimento && new Date(this.mInscricao.DadosPessoais.DataNascimento);
    this.dadosTela.email = this.mInscricao.DadosPessoais.Email;
    this.dadosTela.sexoEscolhido = this.coordenacao.Sexos[this.mInscricao.DadosPessoais.Sexo];
    this.dadosTela.tipoInscricaoEscolhida = this.coordenacao.TiposInscricao[this.mInscricao.TipoInscricao];
    this.dadosTela.cidade = this.mInscricao.DadosPessoais.Cidade;
    this.dadosTela.uf = this.mInscricao.DadosPessoais.Uf;
    this.dadosTela.ehVegetariano = this.mInscricao.DadosPessoais.EhVegetariano;
    this.dadosTela.usaAdocanteDiariamente = this.mInscricao.DadosPessoais.UsaAdocanteDiariamente;
    this.dadosTela.ehDiabetico = this.mInscricao.DadosPessoais.EhDiabetico;
    this.dadosTela.carnesNaoCome = this.mInscricao.DadosPessoais.CarnesNaoCome;
    this.dadosTela.alimentosAlergia = this.mInscricao.DadosPessoais.AlimentosAlergia;
    this.dadosTela.medicamentosUsa = this.mInscricao.DadosPessoais.MedicamentosUsa;
    this.dadosTela.centroEspirita = this.mInscricao.CentroEspirita;
    this.dadosTela.tempoEspirita = this.mInscricao.TempoEspirita;
    this.dadosTela.primeiroEncontro = this.mInscricao.PrimeiroEncontro;
    this.dadosTela.nomeResponsavelCentro = this.mInscricao.NomeResponsavelCentro;
    this.dadosTela.telefoneResponsavelCentro = this.mInscricao.TelefoneResponsavelCentro;
    this.dadosTela.nomeResponsavelLegal = this.mInscricao.NomeResponsavelLegal;
    this.dadosTela.telefoneResponsavelLegal = this.mInscricao.TelefoneResponsavelLegal;
    this.dadosTela.telefoneFixo = this.mInscricao.DadosPessoais.TelefoneFixo;
    this.dadosTela.celular = this.mInscricao.DadosPessoais.Celular;
    this.dadosTela.nomeCracha = this.mInscricao.NomeCracha;

    this.dadosTela.oficinasEscolhidas = this.mInscricao.Oficina;
    this.dadosTela.salasEscolhidas = this.mInscricao.SalasEstudo;
    this.dadosTela.departamentoEscolhido = this.mInscricao.Departamento;
    this.dadosTela.sarais = this.mInscricao.Sarais;
    this.dadosTela.pagamento = this.mInscricao.Pagamento;
    this.dadosTela.observacoes = this.mInscricao.Observacoes;

    if (this.dadosTela.pagamento.ComprovantesBase64 != null)
      this.dadosTela.pagamento.ComprovantesBase64 = this.dadosTela.pagamento.ComprovantesBase64.map(x => 'data:image/jpeg;base64,' + x);

    this.atribuirInscricaoSimples();
  }

  @Input()
  set evento(valor: DTOEventoCompleto) {

    this.mEvento = valor;

    this.dadosTela.descricaoEvento = this.mEvento.Nome;
    this.dadosTela.idadeMinima = this.mEvento.IdadeMinima;
    this.dadosTela.dataMinimaNascimento = new Date(this.mEvento.PeriodoRealizacao.DataInicial);
    this.dadosTela.dataMinimaNascimento.setFullYear(this.dadosTela.dataMinimaNascimento.getFullYear() - this.mEvento.IdadeMinima);
    this.dadosTela.dataInicioEvento = new Date(this.mEvento.PeriodoRealizacao.DataInicial);

    this.atribuirInscricaoSimples();
  }
  get evento() {
    return this.mEvento;
  }

  private atribuirInscricaoSimples(): void {

    if (this.mEvento && this.mInscricao)
      this.dadosTela.inscricaoSimples = { Id: 0, IdEvento: this.mEvento.Id, Nome: this.mInscricao.DadosPessoais.Nome };
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
      this.mEvento.TemOficinas &&
      this.dadosTela.oficinasEscolhidas == null)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu as oficinas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
      this.mEvento.TemOficinas &&
      this.dadosTela.oficinasEscolhidas.EscolhidasParticipante.length != this.mEvento.Oficinas.length)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu todas as oficinas.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
      this.mEvento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
      this.dadosTela.salasEscolhidas == null)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu as salas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
      this.mEvento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
      this.dadosTela.salasEscolhidas.EscolhidasParticipante.length != this.mEvento.SalasEstudo.length)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu todas as salas.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[0] &&
      this.mEvento.TemDepartamentalizacao &&
      this.dadosTela.departamentoEscolhido == null)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu o departamento que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[1] &&
      this.mEvento.TemOficinas &&
      this.dadosTela.oficinasEscolhidas != null &&
      this.dadosTela.oficinasEscolhidas.EscolhidasParticipante != null &&
      this.dadosTela.oficinasEscolhidas.EscolhidasParticipante.length != this.mEvento.Oficinas.length)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu todas as oficinas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[1] &&
      this.mEvento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
      this.dadosTela.salasEscolhidas != null &&
      this.dadosTela.salasEscolhidas.EscolhidasParticipante != null &&
      this.dadosTela.salasEscolhidas.EscolhidasParticipante.length != this.mEvento.SalasEstudo.length)
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu todas as salas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.coordenacao.TiposInscricao[1] &&
      ((this.mEvento.TemDepartamentalizacao || this.mEvento.TemOficinas || this.mEvento.ConfiguracaoSalaEstudo != null) &&
        !((this.mEvento.TemOficinas && this.dadosTela.oficinasEscolhidas != null) ||
          (this.mEvento.TemDepartamentalizacao && this.dadosTela.departamentoEscolhido != null) ||
          (this.mEvento.ConfiguracaoSalaEstudo != null && this.dadosTela.salasEscolhidas != null))))
      this.coordenacao.Alertas.alertarAtencao("Você nos disse que a sua inscrição é de Participante/Trabalhador, mas não escolheu participar em nenhuma atividade!", "Por favor escolha um atividade para fazer parte.");
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
      atualizacao.TipoInscricao = this.coordenacao.TiposInscricao.indexOf(this.dadosTela.tipoInscricao);
      atualizacao.DadosPessoais.Uf = this.dadosTela.uf;
      atualizacao.DadosPessoais.UsaAdocanteDiariamente = this.dadosTela.usaAdocanteDiariamente;
      atualizacao.NomeCracha = this.dadosTela.nomeCracha;
      atualizacao.CentroEspirita = this.dadosTela.centroEspirita;
      atualizacao.NomeResponsavelCentro = this.dadosTela.nomeResponsavelCentro;
      atualizacao.NomeResponsavelLegal = this.dadosTela.nomeResponsavelLegal;
      atualizacao.TelefoneResponsavelCentro = this.dadosTela.telefoneResponsavelCentro;
      atualizacao.TelefoneResponsavelLegal = this.dadosTela.telefoneResponsavelLegal;
      atualizacao.TempoEspirita = this.dadosTela.tempoEspirita;

      if (atualizacao.TipoInscricao != EnumTipoInscricao.Trabalhador) {
        atualizacao.Departamento = this.dadosTela.departamentoEscolhido;
        atualizacao.Oficina = this.dadosTela.oficinasEscolhidas;
        atualizacao.SalasEstudo = this.dadosTela.salasEscolhidas;
      }

      atualizacao.Sarais = this.dadosTela.sarais;
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
