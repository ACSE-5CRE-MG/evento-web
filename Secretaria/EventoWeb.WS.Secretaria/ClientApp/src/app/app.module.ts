import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID, APP_INITIALIZER } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

import { AgGridModule } from 'ag-grid-angular';

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
import { TelaRoteamentoEvento } from './evento/tela-roteamento-evento';
import { WebServiceSala } from './webservices/webservice-salas';

declare function require(url: string);

registerLocaleData(localePt);

@NgModule({
  declarations: [
    CaixaMensagemDlg, DlgEmProcessamento, MenuUsuario, LayoutGeral,
    TelaPrincipal,
    TelaLogin,    
    TelaListaEventos, DlgFormEvento, TelaGestaoEvento, TelaRoteamentoEvento,
    TelaListagemSalas
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
    AgGridModule.withComponents([]),
    RouterModule.forRoot([
      { path: '', component: TelaListaEventos, canActivate: [PermissaoAcessoRota] },
      { path: 'login', component: TelaLogin },
      {
        path: 'evento/:id', component: TelaRoteamentoEvento, canActivate: [PermissaoAcessoRota],
        children: [
          { path: '', component: TelaGestaoEvento, canActivate: [PermissaoAcessoRota] },
          { path: 'salas', component: TelaListagemSalas, canActivate: [PermissaoAcessoRota] }
        ]
      },
      { path: '** ', redirectTo: '' }
    ])
  ],
  entryComponents: [CaixaMensagemDlg, DlgEmProcessamento, MenuUsuario, LayoutGeral,
    TelaListaEventos, DlgFormEvento, TelaRoteamentoEvento,
    TelaListagemSalas],
  providers: [{ provide: LOCALE_ID, useValue: 'pt' }, { provide: MAT_DATE_LOCALE, useValue: 'pt-BR' },
    Alertas, PermissaoAcessoRota, GestaoAutenticacao, ServicoDlgFormEvento,
    WebServiceAutenticacao, WebServiceEventos, WebServiceSala],
  bootstrap: [TelaPrincipal]
})
export class AppModule
{
  constructor(public matIconRegistry: MatIconRegistry, public alertas: Alertas) {

    this.matIconRegistry.registerFontClassAlias('fontawesome', 'fa');
    let configuracao = require('../assets/configuracao.json');
    if (configuracao != null) {
      ConfiguracaoSistemaService.configuracao = configuracao;
    }
    else
      this.alertas.alertarErro("Não há dados de configuração");
  }
}
