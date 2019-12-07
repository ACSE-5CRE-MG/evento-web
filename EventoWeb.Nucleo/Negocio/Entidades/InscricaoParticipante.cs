using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum EnumTipoParticipante { Participante, ParticipanteTrabalhador }

    public enum EnumPagamento { Comprovante, ComprovanteOutraInscricao, Outro }

    public class Pagamento
    {
        private EnumPagamento? m_Forma;
        private IList<ComprovantePagamento> m_Comprovantes;
        private InscricaoParticipante m_Inscricao;

        public Pagamento(InscricaoParticipante inscricao)
        {
            m_Inscricao = inscricao ?? throw new ExcecaoNegocioAtributo("Pagamento", "Inscricao", "Inscrição é obrigatório");
            m_Comprovantes = new List<ComprovantePagamento>();            
        }

        protected Pagamento() { }        

        public virtual InscricaoParticipante Inscricao { get => m_Inscricao; }
        public virtual EnumPagamento? Forma { get => m_Forma; }
        public virtual IEnumerable<ComprovantePagamento> Comprovantes { get => m_Comprovantes; }
        public virtual string Observacao { get; set; }

        public virtual void AtribuirFormaPagamento(EnumPagamento forma, IEnumerable<ArquivoBinario> comprovantes)
        {
            if (forma != EnumPagamento.Comprovante && comprovantes != null && comprovantes.Count() > 0)
                throw new ExcecaoNegocioAtributo("Pagamento", "Forma", "Apenas a forma de pagamento por comprovante pode receber comprovantes");

            if (forma == EnumPagamento.Comprovante && (comprovantes == null || comprovantes.Count() == 0))
                throw new ExcecaoNegocioAtributo("Pagamento", "Forma", "A forma de pagamento por comprovante precisa de comprovantes");

            m_Forma = forma;
            m_Comprovantes.Clear();
            if (comprovantes != null)
            {
                foreach (var item in comprovantes)
                    m_Comprovantes.Add(new ComprovantePagamento(m_Inscricao, item));
            }
        }
    }

    public class ComprovantePagamento : Entidade
    {
        private ArquivoBinario m_ArquivoComprovante;
        private InscricaoParticipante m_Inscricao;

        public ComprovantePagamento(InscricaoParticipante inscricao, ArquivoBinario arquivoComprovante)
        {
            m_Inscricao = inscricao ?? throw new ExcecaoNegocioAtributo("ComprovantePagamento", "Inscricao", "Inscrição é obrigatório");
            m_ArquivoComprovante = arquivoComprovante ?? throw new ExcecaoNegocioAtributo("ComprovantePagamento", "ArquivoComprovante", "ArquivoComprovante é obrigatório");
        }
        protected ComprovantePagamento() { }

        public virtual InscricaoParticipante Inscricao { get => m_Inscricao; }
        public virtual ArquivoBinario ArquivoComprovante { get => m_ArquivoComprovante; }
    }

    public class InscricaoParticipante: Inscricao
    {
        private IList<AAtividadeInscricao> m_Atividades;
        private Pagamento m_Pagamento;

        public InscricaoParticipante(Evento evento, Pessoa pessoa, DateTime dataRecebimento, EnumTipoParticipante tipo) :
            base(evento, pessoa, dataRecebimento)
        {
            m_Atividades = new List<AAtividadeInscricao>();
            Tipo = tipo;
            m_Pagamento = new Pagamento(this);
        }

        protected InscricaoParticipante() { }

        public virtual IEnumerable<AAtividadeInscricao> Atividades { get { return m_Atividades; } }

        public virtual EnumTipoParticipante Tipo { get; set; }

        public virtual Pagamento Pagamento { get => m_Pagamento; }
        public virtual String InstituicoesEspiritasFrequenta { get; set; }
        public virtual string TempoEspirita { get; set; }
        public virtual string NomeResponsavelCentro { get; set; }
        public virtual string TelefoneResponsavelCentro { get; set; }
        public virtual string NomeResponsavelLegal { get; set; }
        public virtual string TelefoneResponsavelLegal { get; set; }
        public virtual string Observacoes { get; set; }


        public virtual void AdicionarAtividade(AAtividadeInscricao atividade)
        {
            if (atividade.Inscrito != this)
                throw new ArgumentException("Atividade é de outra inscrição", "atividade");

            if (m_Atividades.Count(x=> x.GetType() == atividade.GetType()) > 0)
                throw new ArgumentException("Não é possível ter a mesma atividade mais de uma vez", "atividade");

            if (!Evento.TemDepartamentalizacao && atividade.GetType() == typeof(AtividadeInscricaoDepartamento) ||
                !Evento.TemOficinas && atividade.GetType() == typeof(AtividadeInscricaoOficinas) ||
                Evento.ConfiguracaoSalaEstudo != null && atividade.GetType() == typeof(AtividadeInscricaoSalaEstudo))
                throw new ArgumentException("Tipo de atividade não é aceita pelo evento", "atividade");

            m_Atividades.Add(atividade);
        }

        public virtual void RemoverTodasAtividade()
        {
            m_Atividades.Clear();
        }

        public override bool EhValidaIdade(int idade)
        {
            return idade >= Evento.IdadeMinimaInscricaoAdulto;
        }

        protected override void ValidarInscricaoParaSeTornarPendente()
        {
            // ToDo: Escrever código aqui
        }
    }
}
