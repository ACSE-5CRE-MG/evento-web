using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    internal class ParaOndeMoverParticipanteAfrac : IParaOndeMover<Oficina>
    {
        private InscricaoParticipante mParticipante;
        private Oficina mAfracOrigem;

        public ParaOndeMoverParticipanteAfrac(Oficina AfracOrigem, InscricaoParticipante participante)
        {
            mParticipante = participante;
            mAfracOrigem = AfracOrigem;
        }

        public void Para(Oficina afrac)
        {
            if (afrac == null)
                throw new ArgumentNullException("afrac", "Afrac não pode ser vazia.");

            if (afrac.Evento != mParticipante.Evento)
                throw new ArgumentException("Esta afrac é de outro evento.", "afrac");

            if (afrac.EstaNaListaDeParticipantes(mParticipante))
                throw new ArgumentException("Não se pode mover um participante para a afrac em que já se esta.", "afrac");

            mAfracOrigem.RemoverParticipante(mParticipante);
            afrac.AdicionarParticipante(mParticipante);
        }
    }

    internal class MovimentacaoParticipanteAfrac : IMovimentacaoParticipante<Oficina>
    {
        private Oficina mAfrac;
        private AOficinas mAfracs;

        public MovimentacaoParticipanteAfrac(Oficina afrac, AOficinas afracs)
        {
            mAfrac = afrac;
            mAfracs = afracs;
        }

        public void IncluirParticipante(InscricaoParticipante participante)
        {
            if (participante == null)
                throw new ArgumentNullException("participante", "Participante não pode ser nulo.");

            if (participante.Evento != mAfrac.Evento)
                throw new ArgumentException("Este participante é de outro evento.", "participante");

            if (mAfracs.EhParticipanteDeOficinaNoEvento(mAfrac.Evento, participante))
                throw new ArgumentException("Este participante já tem afrac informada.", "participante");

            if (mAfracs.InscritoEhResponsavelPorOficina(mAfrac.Evento, participante))
                throw new ArgumentException("Este participante é responsável de Afrac.", "participante");

            mAfrac.AdicionarParticipante(participante);
        }

        public IParaOndeMover<Oficina> MoverParticipante(InscricaoParticipante participante)
        {
            if (participante == null)
                throw new ArgumentNullException("participante", "Participante não pode ser nulo.");

            if (participante.Evento != mAfrac.Evento)
                throw new ArgumentException("Este participante é de outro evento.", "participante");

            if (mAfracs.InscritoEhResponsavelPorOficina(mAfrac.Evento, participante))
                throw new ArgumentException("Este participante é responsável de Afrac.", "participante");

            if (!mAfracs.EhParticipanteDeOficinaNoEvento(mAfrac.Evento, participante))
                throw new ArgumentException("Este participante não tem afrac informada.", "participante");

            return new ParaOndeMoverParticipanteAfrac(mAfrac, participante);
        }

        public void RemoverParticipante(InscricaoParticipante participante)
        {
            if (participante == null)
                throw new ArgumentNullException("participante", "Participante não pode ser nulo.");

            if (!mAfrac.EstaNaListaDeParticipantes(participante))
                throw new ArgumentException("Participante não existe nesta afrac.");

            mAfrac.RemoverParticipante(participante);
        }
    }

    public class DivisaoManualParticipantesPorAfrac
    {
        private Evento mEvento;
        private AOficinas mAfracs;

        public DivisaoManualParticipantesPorAfrac(Evento evento, AOficinas afracs)
        {
            if (evento == null)
                throw new ArgumentNullException("evento", "Evento não pode ser nulo.");

            if (afracs == null)
                throw new ArgumentNullException("evento", "Afracs não pode ser nulo.");

            if (afracs.HaAOficinasSemResponsavelDefinidoDoEvento(evento))
                throw new InvalidOperationException("Há afracs sem responsável definido.");
            
            mEvento = evento;
            mAfracs = afracs;
        }

        public IMovimentacaoParticipante<Oficina> Afrac(Oficina afrac)
        {
            if (afrac == null)
                throw new ArgumentNullException("afrac", "Afrac não pode ser vazia.");

            if (afrac.Evento != mEvento)
                throw new ArgumentException("Esta Afrac é de outro evento.", "afrac");

            return new MovimentacaoParticipanteAfrac(afrac, mAfracs);
        }
    }
}
