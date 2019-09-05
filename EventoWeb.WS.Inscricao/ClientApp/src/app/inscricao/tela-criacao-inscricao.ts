import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { WsEventos } from '../webservices/wsEventos';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { WsInscricoes } from '../webservices/wsInscricoes';
import { DTODadosConfirmacao, EnumSexo } from './objetos';

@Component({
  selector: 'tela-criacao-inscricao',
  templateUrl: './tela-criacao-inscricao.html',
  styleUrls: ['./tela-criacao-inscricao.scss']
})
export class TelaCriacaoInscricao implements OnInit {  

  dadosTela: DadosTela;

  @ViewChild("grupoValidacao", { static: false })
  grupoValidacao: DxValidationGroupComponent;

  constructor(private rotaAtual: ActivatedRoute, private coordenacao: CoordenacaoCentral,
    private wsEventos: WsEventos, private wsInscricoes: WsInscricoes, private navegadorUrl: Router) { }

  ngOnInit(): void {
    this.dadosTela = new DadosTela();
    this.dadosTela.descricaoEvento = "<Evento>";
    this.dadosTela.sexos = ["Feminino", "Masculino"];
    this.dadosTela.sexoEscolhido = this.dadosTela.sexos[0];

    let dlg = this.coordenacao.Alertas.alertarProcessamento("Carregando dados...");

    this.rotaAtual.params
      .subscribe(
        (parametrosUrl) => {
          let idEvento = parametrosUrl["idevento"];
          
          this.wsEventos.obter(idEvento)
            .subscribe(
              (dadosEvento) => {
                this.dadosTela.descricaoEvento = dadosEvento.Nome;
                this.dadosTela.dataMinimaNascimento = dadosEvento.PeriodoRealizacao.DataInicial;
                this.dadosTela.dataMinimaNascimento.setFullYear(this.dadosTela.dataMinimaNascimento.getFullYear() - dadosEvento.IdadeMinima);
                this.dadosTela.idadeMinima = dadosEvento.IdadeMinima;
                this.dadosTela.idEvento = idEvento;

                dlg.close();
              },
              (erro) => {
                dlg.close();
                this.coordenacao.ProcessamentoErro.processar(erro);
              }
            );
        }
      );
  }

  public clicarContinuar(): void {
     
    let resultadoValidacao = this.grupoValidacao.instance.validate();
    if (!resultadoValidacao.isValid)
      this.coordenacao.Alertas.alertarAtencao("Nâo deu para continuar!!", "Ops, acho que alguns dados informados não estão legais!");
    else {

      let dlg = this.coordenacao.Alertas.alertarProcessamento("Criando inscrição...");

      this.wsInscricoes.criar(this.dadosTela.idEvento, {
        DataNascimento: this.dadosTela.dataNascimento,
        Email: this.dadosTela.email,
        Nome: this.dadosTela.nome,
        Sexo: (this.dadosTela.sexoEscolhido[0] == this.dadosTela.sexoEscolhido ? EnumSexo.Feminino : EnumSexo.Masculino)
      })
        .subscribe(
          (confirmacao) => {
            dlg.close();
            this.navegadorUrl.navigate(['validar/' + confirmacao.IdInscricao]);
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
  idEvento: number;
  dadosConfirmacao: DTODadosConfirmacao;
  sexos: string[];
  sexoEscolhido: string;
}
