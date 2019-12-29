﻿using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class ASalasEstudo: ARepositorio<SalaEstudo>
    {
        public ASalasEstudo(IPersistencia<SalaEstudo> persistencia) : base(persistencia)
        {
        }

        public abstract SalaEstudo ObterPorId(int idEvento, int id);
        public abstract IList<SalaEstudo> ListarTodasPorEvento(int idEvento);
        public abstract IList<SalaEstudo> ListarTodasSalasEstudoComParticipantesDoEvento(Evento evento);
        public abstract Boolean EhCoordenadorDeSalaNoEvento(Evento evento, InscricaoParticipante participante);
        public abstract bool HaSalasSemCoordenadorDefinidoDoEvento(Evento evento);
        public abstract bool EhParticipanteDeSalaNoEvento(Evento evento, InscricaoParticipante participante);
        public abstract IList<InscricaoParticipante> ListarParticipantesSemSalaEstudoNoEvento(Evento evento);
        public abstract int ContarTotalSalas(Evento evento);
        public abstract SalaEstudo BuscarSalaDoInscrito(int idEvento, int idInscricao);

        protected abstract bool HaSalaComFaixaEtariaDefinida(SalaEstudo sala);
        protected abstract bool HaParticipatesEmOutraSala(SalaEstudo sala);
        protected abstract bool FoiEscolhidaInscricao(SalaEstudo sala);

        public override void Atualizar(SalaEstudo objeto)
        {
            ValidarFaixaEtaria(objeto);
            ValidarSeHaParticipantesEmOutraSala(objeto);
            base.Atualizar(objeto);
        }

        public override void Incluir(SalaEstudo objeto)
        {
            ValidarFaixaEtaria(objeto);
            ValidarSeHaParticipantesEmOutraSala(objeto);
            base.Incluir(objeto);
        }

        public override void Excluir(SalaEstudo objeto)
        {
            if (objeto.Evento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
                FoiEscolhidaInscricao(objeto))
                throw new ERepositorio("Não é possível escolher uma sala que já foi escolhida nas inscrições");

            base.Excluir(objeto);
        }

        private void ValidarFaixaEtaria(SalaEstudo sala)
        {
            if (sala.FaixaEtaria != null && HaSalaComFaixaEtariaDefinida(sala))
                throw new ERepositorio("Já existe outra sala de estudo com faixa etária definida. O sistema somente aceita uma sala com faixa etária.");
        }

        private void ValidarSeHaParticipantesEmOutraSala(SalaEstudo sala)
        {
            if (HaParticipatesEmOutraSala(sala))
                throw new ERepositorio("Existem participantes nesta sala, como coordenadores ou participantes, que estão em outra sala.");
        }
    }
}
