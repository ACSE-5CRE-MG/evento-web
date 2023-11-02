import { Component } from '@angular/core';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ActivatedRoute } from '@angular/router';
import { WebServiceEtiquetas } from '../webservices/webservice-etiquetas';
import { TelaEtiquetaBase } from './tela-etiqueta-base';
import { WebServiceEventos } from '../webservices/webservice-eventos';

@Component({
  selector: 'tela-etiqueta-cracha',
  templateUrl: './tela-etiqueta-base.html',
  styleUrls: ["./tela-etiqueta-base.scss"]
})
export class TelaEtiquetaCracha extends TelaEtiquetaBase {

  constructor(
    protected wsEventos: WebServiceEventos,
    protected wsEtiquetas: WebServiceEtiquetas,
    protected alertas: Alertas,
    protected roteador: ActivatedRoute)    
  {
    super("Geração de Etiquetas do Crachá", "fas fa-user-tag", wsEventos, wsEtiquetas, alertas, roteador);
  }

  
  public clicarGerar(): void {
    let dlg = this.alertas.alertarProcessamento("Gerando etiquetas...");

    let inscricoesGerar = this
      .inscricoes
      .filter(x => this.inscricoesSelecionadas.filter(y => y == x.Id).length > 0);      

    this.wsEtiquetas.gerarEtiquetasCracha(inscricoesGerar)
      .subscribe(
        relatorioGerado => {
          dlg.close();
          window.open(URL.createObjectURL(relatorioGerado), '_blank');
        },
        erro => {
          dlg.close();
          this.alertas.alertarErro(erro);
        }
      );    
  }
}
