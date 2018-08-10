import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { HttpModule, Http, Response, Headers, RequestOptions, ResponseContentType } from '@angular/http';
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

import { FlexLayoutModule } from "@angular/flex-layout";

import { TelaPrincipal } from './tela-principal';
import { TelaLogin } from './login/tela-login'

@NgModule({
  declarations: [
    TelaPrincipal,
    TelaLogin
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
    RouterModule.forRoot([])
  ],
  providers: [{ provide: LOCALE_ID, useValue: 'pt' }, { provide: MAT_DATE_LOCALE, useValue: 'pt-BR' }],
  bootstrap: [TelaPrincipal]
})
export class AppModule
{
  constructor(public matIconRegistry: MatIconRegistry, /*public alertas: Alertas,*/ public http: Http) {

    this.matIconRegistry.registerFontClassAlias('fontawesome', 'fa');

    /*let opRequisicao = new RequestOptions();
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
          this.alertas.alertarSemConfiguracaoInicial("Não há dados de configuração");
      },
        error => {
          this.alertas.alertarSemConfiguracaoInicial(error);
        });*/
  }
}
