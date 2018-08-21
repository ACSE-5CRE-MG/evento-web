import { Component, Input } from '@angular/core';

@Component({
  selector: 'layout-barra-ferramenta',
  styleUrls: ['./layout-barra-ferramenta.scss'],
  templateUrl: './layout-barra-ferramenta.html',
})
export class LayoutCadastro {

  @Input('corToolbar') corToolbar: string = 'primary';
}
