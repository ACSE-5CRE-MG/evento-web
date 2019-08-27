import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'tela-criacao-inscricao',
  templateUrl: './tela-criacao-inscricao.html',
  styleUrls: ['./tela-criacao-inscricao.scss']
})
export class TelaCriacaoInscricao implements OnInit {  

  dadosTela: DadosTela;

  ngOnInit(): void {
    this.dadosTela = new DadosTela();
    this.dadosTela.descricaoEvento = "<Evento>"
  }
}

class DadosTela {
  nome: string;
  dataNascimento: Date;
  email: string;
  descricaoEvento: string;
}
