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
  DxSelectBoxModule
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
import { WsInscricoes } from './webservices/wsInscricoes';
import { TelaCodigoInscricao } from './inscricao/tela-codigo-inscricao';
import { TelaInscricao } from './inscricao/tela-inscricao';
import { PermissaoAcessoInscricao } from './permissao-acesso-inscricao';
import { WsManutencaoInscricoes } from './webservices/wsManutencaoInscricoes';
import { ComponenteOficinas, ComponenteOficinaParticipante, ComponenteOficinaCoordenador } from './inscricao/atividades/comp-oficinas';
import { ComponenteSalas, ComponenteSalasParticipanteComEscolha, ComponenteSalasParticipanteSemEscolha, ComponenteSalaCoordenador } from './inscricao/atividades/comp-salas';
import { ComponenteDepartamentos } from './inscricao/atividades/comp-departamentos';

declare function require(url: string);

registerLocaleData(localePt);

let ptMessages = require("devextreme/localization/messages/pt.json");
loadMessages(ptMessages);
locale('pt');

@NgModule({
    declarations: [
        TelaBase, TelaPrincipal, TelaCriacaoInscricao, TelaPesquisaInscricao, TelaCodigoInscricao, TelaInscricao,
        CaixaMensagemDlg, DlgEmProcessamento, LayoutGeral,
        ComponenteOficinas, ComponenteOficinaParticipante, ComponenteOficinaCoordenador,
        ComponenteSalas, ComponenteSalasParticipanteComEscolha, ComponenteSalasParticipanteSemEscolha, ComponenteSalaCoordenador,
        ComponenteDepartamentos
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
        RouterModule.forRoot([
            { path: '', component: TelaPrincipal },
            { path: 'comecar/:idevento', component: TelaCriacaoInscricao },
            { path: 'pesquisar', component: TelaPesquisaInscricao },
            { path: 'validar/:idinscricao', component: TelaCodigoInscricao },
            { path: 'inscricao/:idinscricao', component: TelaInscricao, canActivate: [PermissaoAcessoInscricao], },
            { path: '**', redirectTo: '' }
        ]),
        BrowserAnimationsModule
    ],
    entryComponents: [CaixaMensagemDlg, DlgEmProcessamento],
    providers: [{ provide: LOCALE_ID, useValue: 'pt' },
        CoordenacaoCentral, WsEventos, WsInscricoes, WsManutencaoInscricoes, PermissaoAcessoInscricao
    ],
    bootstrap: [TelaBase]
})
export class AppModule { }
