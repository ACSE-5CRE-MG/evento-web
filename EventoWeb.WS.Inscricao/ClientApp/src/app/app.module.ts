import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

import { FlexLayoutModule } from "@angular/flex-layout";

import {  
  MatSidenavModule,
  MatDialogModule,
  MatToolbarModule,
  MatButtonModule,
  MatDividerModule
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
  DxValidationGroupModule
} from 'devextreme-angular';

import { locale, loadMessages } from 'devextreme/localization';
import 'devextreme-intl';

import { TelaBase } from './tela-base';
import { TelaPrincipal } from './principal/tela-principal';
import { LayoutGeral } from './componentes/layout-geral/layout-geral';
import { DlgEmProcessamento } from './componentes/alertas-dlg/alertas';
import { CaixaMensagemDlg } from './componentes/alertas-dlg/caixa-mensagem-dlg';
import { CoordenacaoCentral } from './componentes/central/coordenacao-central';
import { TelaCriacaoInscricao } from './inscricao/tela-criacao-inscricao';
import { TelaPesquisaInscricao } from './inscricao/tela-pesquisa-inscricao';
import { WsEventos } from './webservices/wsEventos';

declare function require(url: string);

registerLocaleData(localePt);

let ptMessages = require("devextreme/localization/messages/pt.json");
loadMessages(ptMessages);
locale('pt');

@NgModule({
  declarations: [
    TelaBase, TelaPrincipal, TelaCriacaoInscricao, TelaPesquisaInscricao,
    CaixaMensagemDlg, DlgEmProcessamento, LayoutGeral,
  ],
  imports: [
    BrowserModule,
    FlexLayoutModule,
    MatSidenavModule,
    MatDialogModule,
    MatToolbarModule,
    MatButtonModule,
    MatDividerModule,
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
    RouterModule.forRoot([
      { path: '', component: TelaPrincipal },
      { path: 'comecar/:idevento', component: TelaCriacaoInscricao },
      { path: 'pesquisar', component: TelaPesquisaInscricao }
    ]),
    BrowserAnimationsModule
  ],
  entryComponents: [CaixaMensagemDlg, DlgEmProcessamento],
  providers: [{ provide: LOCALE_ID, useValue: 'pt' },
    CoordenacaoCentral, WsEventos
  ],
  bootstrap: [TelaBase]
})
export class AppModule { }
