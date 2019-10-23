import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { DTOInscricaoCompleta, DTOInscricaoDadosPessoais, EnumSexo, EnumTipoInscricao, DTOInscricaoOficina, DTOInscricaoSalaEstudo, DTOSarau, DTOInscricaoSimplificada, DTOCrianca, DTOInscricaoAtualizacao } from '../inscricao/objetos';
import { Periodo, DTOEventoCompleto, EnumPublicoEvangelizacao, EnumModeloDivisaoSalasEstudo } from '../principal/objetos';

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
            inscricao.Evento.SalasEstudo = [{ Descricao: "Sala 1", Id: 1 }, { Descricao: "Sala 2", Id: 2 }, { Descricao: "Sala 3", Id: 3 }];
            inscricao.Evento.Departamentos = [{ Descricao: "Departamento 1", Id: 1 }, { Descricao: "Departamento 2", Id: 2 }, { Descricao: "Departamento 3", Id: 3 }];
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
            inscricao.Evento.CnfSalaEstudo = EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao;
            inscricao.Evento.ValorInscricaoAdulto = 60.0;
            inscricao.Evento.ValorInscricaoCrianca = 30.0;
            inscricao.Id = 1;
            //inscricao.Oficina = new DTOInscricaoOficina();
            //inscricao.SalasEstudo = new DTOInscricaoSalaEstudo();

            x.next(inscricao);
        });
    }

    atualizar(id: number, inscricao: DTOInscricaoAtualizacao): Observable<void> {
        return new Observable<void>((x) => {

            x.next();
        });
    }

    obterSarau(idEvento: number, codigoSarau: string): Observable<DTOSarau> {
        return new Observable<DTOSarau>((x) => {

            let sarau = new DTOSarau();
            sarau.Id = 1;
            sarau.DuracaoMin = 10;
            sarau.Participantes = [];
            sarau.Participantes.push({ Id: 1, Nome: "João da Silva", IdEvento: 1 });
            sarau.Tipo = "Música";

            x.next(sarau);
        });
    }

    obterCrianca(IdEvento: number, codigo: string): Observable<DTOCrianca> {
        return new Observable<DTOCrianca>((x) => {

            let crianca = new DTOCrianca();
            crianca.Id = 1;
            crianca.Nome = "Criança 1";
            crianca.Responsaveis = [];
            crianca.Responsaveis.push({ Id: 1, Nome: "João da Silva", IdEvento: 1 });

            x.next(crianca);
        });
    }

}
