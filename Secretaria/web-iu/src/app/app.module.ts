import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { HttpModule, Http, Response, Headers, RequestOptions, ResponseContentType } from '@angular/http';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

import { Observable } from 'rxjs';

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

import { FlexLayoutModule } from "@angular/flex-layout";

import { CookieModule } from 'ngx-cookie';

import { CaixaMensagemDlg } from './componentes/alertas-dlg/caixa-mensagem-dlg';
import { DlgEmProcessamento, Alertas } from './componentes/alertas-dlg/alertas';

import { WebServiceAutenticacao } from './webservices/webservice-autenticacao';
import { WebServiceEventos } from './webservices/webservice-eventos';

import { PermissaoAcessoRota } from './seguranca/permissao-acesso-rota';
import { GestaoAutenticacao } from './seguranca/gestao-autenticacao';

import { TelaLogin } from './login/tela-login';
import { TelaPrincipal } from './tela-principal';

import { LayoutCadastro } from './componentes/layout/layout-barra-ferramenta';
import { LayoutBase } from './componentes/layout-base/layout-base';

import { TelaListaEventos } from './evento/tela-lista-eventos';
import { ConfiguracaoSistemaService } from './configuracao-sistema-service';


@NgModule({
  declarations: [
    CaixaMensagemDlg, DlgEmProcessamento,
    TelaPrincipal,
    TelaLogin,
    LayoutCadastro,
    LayoutBase,
    TelaListaEventos
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpModule,
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
    FlexLayoutModule,
    CookieModule.forRoot(),
    RouterModule.forRoot([
      { path: 'login', component: TelaLogin },
      { path: '', component: TelaListaEventos, canActivate: [PermissaoAcessoRota] },
      { path: '** ', redirectTo: '' }
    ])
  ],
  entryComponents: [CaixaMensagemDlg, DlgEmProcessamento, LayoutCadastro, LayoutBase, TelaListaEventos],
  providers: [{ provide: LOCALE_ID, useValue: 'pt' }, { provide: MAT_DATE_LOCALE, useValue: 'pt-BR' },
    Alertas, PermissaoAcessoRota, GestaoAutenticacao,
    WebServiceAutenticacao, WebServiceEventos],
  bootstrap: [TelaPrincipal]
})
export class AppModule
{
  constructor(public matIconRegistry: MatIconRegistry, public alertas: Alertas, public http: Http) {

    this.matIconRegistry.registerFontClassAlias('fontawesome', 'fa');

    let opRequisicao = new RequestOptions();
    opRequisicao.headers = new Headers();
    opRequisicao.headers.append('Content-Type', 'application/json');

    this.http
      .get('/assets/configuracao.json', opRequisicao)
      .map(res => { return res.json(); })
      .catch(erro => { return Observable.throw(erro.toString()); })
      .subscribe(configuracao => {
        if (configuracao != null) {
          ConfiguracaoSistemaService.configuracao = configuracao;
        }
        else
          this.alertas.alertarErro("Não há dados de configuração");

        console.log(configuracao);
      },
      error => {
        console.log(error);
          this.alertas.alertarErro(error);
        });
  }
}
