import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { DTOInscricaoCompleta, DTOInscricaoDadosPessoais, EnumSexo, EnumTipoInscricao, DTOInscricaoOficina } from '../inscricao/objetos';
import { DTOEventoListagem, Periodo, DTOEventoCompleto, EnumPublicoEvangelizacao, EnumModeloDivisaoSalasEstudo } from '../principal/objetos';

@Injectable()
export class WsManutencaoInscricoes {

  
  obterInscricaoCompleta(idInscricao: number): Observable<DTOInscricaoCompleta> {
    return new Observable<DTOInscricaoCompleta>((x) => {

      let inscricao = new DTOInscricaoCompleta();
      inscricao.CentroEspirita = "Centro";
      inscricao.DadosPessoais = new DTOInscricaoDadosPessoais();
      inscricao.DadosPessoais.Cidade = "Cidade";
      inscricao.DadosPessoais.Email = "joao@uol.com.br";
      inscricao.DadosPessoais.Nome = "João da Silva";
      inscricao.DadosPessoais.Sexo = EnumSexo.Masculino;
      inscricao.DadosPessoais.TipoInscricao = EnumTipoInscricao.Participante;
      inscricao.DadosPessoais.Uf = "MG";
      inscricao.Evento = new DTOEventoCompleto();
      inscricao.Evento.Id = 1;
      inscricao.Evento.IdadeMinima = 13;
      inscricao.Evento.Nome = "Evento Teste";
      inscricao.Evento.Oficinas = [{ Descricao: "Oficina 1", Id: 1 }, { Descricao: "Oficina 2", Id: 2 }, { Descricao: "Oficina 3", Id: 3 }];
      inscricao.Evento.PeriodoInscricao = new Periodo();
      inscricao.Evento.PeriodoInscricao.DataFinal = new Date();
      inscricao.Evento.PeriodoInscricao.DataInicial = inscricao.Evento.PeriodoInscricao.DataFinal;
      inscricao.Evento.PeriodoRealizacao = new Periodo();
      inscricao.Evento.PeriodoRealizacao.DataFinal = inscricao.Evento.PeriodoInscricao.DataFinal;
      inscricao.Evento.PeriodoRealizacao.DataInicial = inscricao.Evento.PeriodoRealizacao.DataFinal;
      inscricao.Evento.TemDepartamentos = true;
      inscricao.Evento.TemOficinas = true;
      inscricao.Evento.TemSarau = true;
      inscricao.Evento.CnfEvangelizacao = EnumPublicoEvangelizacao.Todos;
      inscricao.Evento.CnfSalaEstudo = EnumModeloDivisaoSalasEstudo.PorIdadeCidade;
      inscricao.Id = 1;
      inscricao.Oficina = new DTOInscricaoOficina();

      x.next(inscricao);
    });
  }

  atualizar(inscricao: DTOInscricaoCompleta): Observable<void> {
    return new Observable<void>((x) => {
      
      x.next();
    });
  }
}
