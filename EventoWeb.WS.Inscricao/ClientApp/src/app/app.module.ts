import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID, Injectable, APP_INITIALIZER } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

import { FlexLayoutModule } from "@angular/flex-layout";

import {
  MatSidenavModule,
  MatDialogModule,
  MatToolbarModule,
  MatButtonModule,
  MatDividerModule,
  MatCardModule,
  MatTabsModule
} from '@angular/material';

import {
  DxDataGridModule,
  DxTextBoxModule,
  DxNumberBoxModule,
  DxDateBoxModule,
  DxDropDownBoxModule,
  DxListModule,
  DxRadioGroupModule,
  DxCheckBoxModule,
  DxPopoverModule,
  DxTextAreaModule,
  DxValidatorModule,
  DxValidationGroupModule,
  DxSelectBoxModule,
  DxFileUploaderModule,
  DxGalleryModule
} from 'devextreme-angular';

import { locale, loadMessages } from 'devextreme/localization';

import { TelaBase } from './tela-base';
import { TelaPrincipal } from './principal/tela-principal';
import { LayoutGeral } from './componentes/layout-geral/layout-geral';
import { DlgEmProcessamento } from './componentes/alertas-dlg/alertas';
import { CaixaMensagemDlg } from './componentes/alertas-dlg/caixa-mensagem-dlg';
import { CoordenacaoCentral } from './componentes/central/coordenacao-central';
import { TelaCriacaoInscricao } from './inscricao/tela-criacao-inscricao';
import { TelaPesquisaInscricao } from './inscricao/tela-pesquisa-inscricao';
import { WsEventos } from './webservices/wsEventos';
import { WsInscricoes } from './webservices/wsInscricoes';
import { TelaCodigoInscricao } from './inscricao/tela-codigo-inscricao';
import { TelaInscricaoAtualizacao, TelaInscricaoInclusao } from './inscricao/tela-inscricao';
import { PermissaoAcessoInscricao } from './permissao-acesso-inscricao';
import { WsManutencaoInscricoes } from './webservices/wsManutencaoInscricoes';
import { ComponenteOficinas, ComponenteOficinaParticipante, ComponenteOficinaCoordenador } from './inscricao/atividades/comp-oficinas';
import { ComponenteSalas, ComponenteSalasParticipanteComEscolha, ComponenteSalasParticipanteSemEscolha, ComponenteSalaCoordenador } from './inscricao/atividades/comp-salas';
import { ComponenteDepartamentos } from './inscricao/atividades/comp-departamentos';
import { ComponenteSarau, DlgSarauCodigo, DlgSarauFormulario, DialogosSarau } from './inscricao/atividades/comp-sarau';
import { ComponentePagamento } from './inscricao/pagamento/comp-pagamento';
import { ClienteWs } from './webservices/cliente-ws';
import { Observable } from 'rxjs';
import { Configuracao, ConfiguracaoSistemaService } from './configuracao-sistema-service';
import { ComponenteContrato, SanitizeHtmlPipe } from './contrato/comp-contrato';
import { DlgContrato, DialogoContrato } from './contrato/dlg-contrato';
import { DlgValidacaoEmail, DialogoValidacaoEmail } from './inscricao/dlg-validacao-email';
import { CompFormInscricao } from './inscricao/comp-form-inscricao';
import { TelaInscricaoAtualizacaoInfantil, TelaInscricaoInclusaoInfantil } from './inscricao/tela-inscricao-infantil';
import { DlgInscricaoAdultoCodigo, DialogosInscricao } from './inscricao/dlg-inscricao-adulto-codigo';
import { CompFormInscricaoInfantil } from './inscricao/comp-form-inscricao-infantil';

declare function require(url: string);

registerLocaleData(localePt);

let ptMessages = require("devextreme/localization/messages/pt.json");
loadMessages(ptMessages);
locale('pt');

@Injectable()
export class AppLoadService {

  constructor(private http: HttpClient) { }

  initializeApp(): Promise<any> {

    let controle = new Observable((observador) => {
      this.http.get<Configuracao>('assets/configuracao.json')
        .subscribe(
          (cnf) => {
            ConfiguracaoSistemaService.configuracao = cnf;
            observador.complete();
          },
          (erro) => observador.error(erro)
        );
    })
      .toPromise();

    return controle;
  }
}

export function init_app(appLoadService: AppLoadService) {
  return () => appLoadService.initializeApp();
}

@NgModule({
  declarations: [
    TelaBase, TelaPrincipal, TelaCriacaoInscricao, TelaPesquisaInscricao, TelaCodigoInscricao, TelaInscricaoInclusao, TelaInscricaoAtualizacao,
    CaixaMensagemDlg, DlgEmProcessamento, LayoutGeral,
    ComponenteOficinas, ComponenteOficinaParticipante, ComponenteOficinaCoordenador,
    ComponenteSalas, ComponenteSalasParticipanteComEscolha, ComponenteSalasParticipanteSemEscolha, ComponenteSalaCoordenador,
    ComponenteDepartamentos, ComponenteSarau, DlgSarauCodigo, DlgSarauFormulario,
    ComponentePagamento, CompFormInscricao,
    ComponenteContrato, SanitizeHtmlPipe, DlgContrato,
    DlgValidacaoEmail, TelaInscricaoInclusaoInfantil, TelaInscricaoAtualizacaoInfantil,
    CompFormInscricaoInfantil, DlgInscricaoAdultoCodigo 
  ],
  imports: [
    BrowserModule,
    FlexLayoutModule,
    MatSidenavModule,
    MatDialogModule,
    MatToolbarModule,
    MatButtonModule,
    MatDividerModule,
    MatCardModule,
    MatTabsModule,
    DxDataGridModule,
    DxTextBoxModule,
    DxNumberBoxModule,
    DxDateBoxModule,
    DxDropDownBoxModule,
    DxListModule,
    DxRadioGroupModule,
    DxCheckBoxModule,
    DxPopoverModule,
    DxTextAreaModule,
    DxValidatorModule,
    DxValidationGroupModule,
    DxSelectBoxModule,
    DxFileUploaderModule,
    DxGalleryModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: '', component: TelaPrincipal },
      { path: 'comecar/:idevento', component: TelaCriacaoInscricao },
      { path: 'pesquisar', component: TelaPesquisaInscricao },
      { path: 'validar/:idinscricao', component: TelaCodigoInscricao },
      { path: 'inscricao/:idinscricao', component: TelaInscricaoAtualizacao, canActivate: [PermissaoAcessoInscricao], },
      { path: 'criar-inscricao/:idevento', component: TelaInscricaoInclusao, },
      { path: 'inscricao-infantil/:idinscricao', component: TelaInscricaoAtualizacaoInfantil, canActivate: [PermissaoAcessoInscricao], },
      { path: 'criar-inscricao-infantil/:idevento', component: TelaInscricaoInclusaoInfantil, },
      { path: '**', redirectTo: '' }
    ]),
    BrowserAnimationsModule
  ],
  entryComponents: [CaixaMensagemDlg, DlgEmProcessamento, DlgSarauCodigo, DlgSarauFormulario, DlgContrato, DlgValidacaoEmail, DlgInscricaoAdultoCodigo],
  providers: [
    AppLoadService,
    { provide: LOCALE_ID, useValue: 'pt' },
    { provide: APP_INITIALIZER, useFactory: init_app, deps: [AppLoadService], multi: true },
    CoordenacaoCentral, WsEventos, ClienteWs, WsInscricoes, WsManutencaoInscricoes, PermissaoAcessoInscricao, DialogosSarau, DialogoContrato,
    DialogoValidacaoEmail, DialogosInscricao
  ],
  bootstrap: [TelaBase]
})
export class AppModule { }
