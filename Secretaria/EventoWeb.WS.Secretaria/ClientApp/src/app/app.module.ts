import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID, APP_INITIALIZER, Injectable } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

import {
  MatCardModule,
  MatIconModule,
  MatIconRegistry,
  MatMenuModule,
  MatButtonModule,
  MatListModule,
  MatTooltipModule,
  MatInputModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatDialogModule,
  MatToolbarModule,
  MatButtonToggleModule,
  MatExpansionModule,
  MatGridListModule,
  MatSidenavModule,
  MatTabsModule,
  MatRadioModule,
  MatCheckboxModule,
  MAT_DATE_LOCALE
} from '@angular/material';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';

import { MatDatetimepickerModule } from '@mat-datetimepicker/core';
import { MatMomentDatetimeModule } from '@mat-datetimepicker/moment';

import { FlexLayoutModule } from "@angular/flex-layout";

import { TextMaskModule } from 'angular2-text-mask';

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

import { CaixaMensagemDlg } from './componentes/alertas-dlg/caixa-mensagem-dlg';
import { DlgEmProcessamento, Alertas } from './componentes/alertas-dlg/alertas';
import { LayoutGeral } from './componentes/layout-geral/layout-geral';
import { MenuUsuario } from './componentes/menu-usuario/menu-usuario';

import { WebServiceAutenticacao } from './webservices/webservice-autenticacao';
import { WebServiceEventos } from './webservices/webservice-eventos';

import { PermissaoAcessoRota } from './seguranca/permissao-acesso-rota';
import { GestaoAutenticacao } from './seguranca/gestao-autenticacao';

import { TelaLogin } from './login/tela-login';
import { TelaPrincipal } from './tela-principal';

import { TelaListaEventos } from './evento/tela-lista-eventos';
import { DlgFormEvento, ServicoDlgFormEvento } from './evento/dlg-form-evento';
import { TelaGestaoEvento } from './evento/tela-gestao-evento';

import { ConfiguracaoSistemaService, Configuracao } from './configuracao-sistema-service';
import { TelaListagemSalas } from './sala-estudo/tela-listagem-salas';
import { TelaRoteamentoEvento, ServicoEventoSelecionado } from './evento/tela-roteamento-evento';
import { WebServiceSala } from './webservices/webservice-salas';
import { TelaListagemInscricoes } from './inscricao/tela-lista-inscricoes';
import { WebServiceInscricoes } from './webservices/webservice-inscricoes';
import { TelaInscricao } from './inscricao/tela-inscricao';
import { ComponenteOficinas, ComponenteOficinaParticipante, ComponenteOficinaCoordenador } from './inscricao/atividades/comp-oficinas';
import { ComponenteSalas, ComponenteSalasParticipanteComEscolha, ComponenteSalasParticipanteSemEscolha, ComponenteSalaCoordenador } from './inscricao/atividades/comp-salas';
import { ComponenteDepartamentos } from './inscricao/atividades/comp-departamentos';
import { ComponenteSarau, DlgSarauCodigo, DlgSarauFormulario, DialogosSarau } from './inscricao/atividades/comp-sarau';
import { ComponenteCriancas, DlgCriancaCodigo, DlgCriancaFormulario, DialogosCrianca } from './inscricao/criancas/comp-criancas';
import { ComponentePagamento } from './inscricao/pagamento/comp-pagamento';
import { Observable } from 'rxjs';
import { DialogosSala, DlgFormSala } from './sala-estudo/dlg-form-sala';

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
    CaixaMensagemDlg, DlgEmProcessamento, MenuUsuario, LayoutGeral,
    TelaPrincipal,
    TelaLogin,
    TelaListaEventos, DlgFormEvento, TelaGestaoEvento, TelaRoteamentoEvento,
    TelaListagemSalas, DlgFormSala,
    TelaListagemInscricoes,
    TelaInscricao,
    ComponenteOficinas, ComponenteOficinaParticipante, ComponenteOficinaCoordenador,
    ComponenteSalas, ComponenteSalasParticipanteComEscolha, ComponenteSalasParticipanteSemEscolha, ComponenteSalaCoordenador,
    ComponenteDepartamentos, ComponenteSarau, DlgSarauCodigo, DlgSarauFormulario,
    ComponenteCriancas, DlgCriancaCodigo, DlgCriancaFormulario,
    ComponentePagamento
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    MatCardModule,
    MatIconModule,
    MatMenuModule,
    MatButtonModule,
    MatListModule,
    MatTooltipModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule,
    MatToolbarModule,
    MatButtonToggleModule,
    MatExpansionModule,
    MatGridListModule,
    MatSidenavModule,
    MatTabsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatRadioModule,
    MatCheckboxModule,
    MatMomentDatetimeModule,
    MatDatetimepickerModule,
    FlexLayoutModule,
    TextMaskModule,
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
    RouterModule.forRoot([
      { path: '', component: TelaListaEventos, canActivate: [PermissaoAcessoRota] },
      { path: 'login', component: TelaLogin },
      {
        path: 'evento/:id', component: TelaRoteamentoEvento, canActivate: [PermissaoAcessoRota],
        children: [
          { path: '', component: TelaGestaoEvento, canActivate: [PermissaoAcessoRota] },
          { path: 'salas', component: TelaListagemSalas, canActivate: [PermissaoAcessoRota] },
          { path: 'inscricoes', component: TelaListagemInscricoes, canActivate: [PermissaoAcessoRota] },
          { path: 'inscricoes/:idInscricao/editar', component: TelaInscricao, canActivate: [PermissaoAcessoRota] },
        ]
      },
      { path: '** ', redirectTo: '' }
    ], { enableTracing: false })
  ],
  entryComponents: [CaixaMensagemDlg, DlgEmProcessamento, MenuUsuario, LayoutGeral,
    TelaListaEventos, DlgFormEvento, TelaRoteamentoEvento,
    TelaListagemSalas, DlgFormSala,
    DlgSarauCodigo, DlgSarauFormulario, DlgCriancaCodigo, DlgCriancaFormulario],
  providers: [
    AppLoadService,
    { provide: LOCALE_ID, useValue: 'pt' },
    { provide: MAT_DATE_LOCALE, useValue: 'pt-BR' },
    { provide: APP_INITIALIZER, useFactory: init_app, deps: [AppLoadService], multi: true },
    Alertas, PermissaoAcessoRota, GestaoAutenticacao, ServicoDlgFormEvento,
    WebServiceAutenticacao, WebServiceEventos, WebServiceSala, WebServiceInscricoes, ServicoEventoSelecionado,
    DialogosSarau, DialogosCrianca, DialogosSala],
  bootstrap: [TelaPrincipal]
})
export class AppModule {
}
