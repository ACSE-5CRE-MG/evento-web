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

import { TelaBase } from './tela-base';
import { TelaPrincipal } from './principal/tela-principal';
import { LayoutGeral } from './componentes/layout-geral/layout-geral';
import { DlgEmProcessamento } from './componentes/alertas-dlg/alertas';
import { CaixaMensagemDlg } from './componentes/alertas-dlg/caixa-mensagem-dlg';
import { CoordenacaoCentral } from './componentes/central/coordenacao-central';
import { TelaCriacaoInscricao } from './inscricao/tela-criacao-inscricao';

registerLocaleData(localePt);

@NgModule({
  declarations: [
    TelaBase, TelaPrincipal, TelaCriacaoInscricao,
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
    RouterModule.forRoot([
      { path: '', component: TelaPrincipal },
      { path: 'criarinscricao/:idevento', component: TelaCriacaoInscricao }
    ]),
    BrowserAnimationsModule
  ],
  providers: [{ provide: LOCALE_ID, useValue: 'pt' },
    CoordenacaoCentral
  ],
  bootstrap: [TelaBase]
})
export class AppModule { }
