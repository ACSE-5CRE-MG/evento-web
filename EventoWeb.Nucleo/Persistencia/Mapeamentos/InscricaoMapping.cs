using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class InscricaoMapping: ClassMapping<Inscricao>
    {
        public InscricaoMapping()
        {
            Table("INSCRICOES");

            Discriminator(d =>
            {
                d.Column("TIPO_INSCRICAO");
                d.Length(30);
            });

            Abstract(true);

            Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_INSCRICAO");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_INSCRICAO" });
                });
            });

            Property(x => x.ConfirmadoNoEvento, m =>
              {
                  m.Access(Accessor.Property);
                  m.Column("CONFIRMADO");
                  m.NotNullable(true);
              });

            Property(x => x.DataRecebimento, m =>
              {
                  m.Access(Accessor.Property);
                  m.Column("DATA_RECEBIMENTO");
                  m.NotNullable(true);
              });

            Property(x => x.DormeEvento, m =>
            {
                m.Access(Accessor.Property);
                m.Column("DORMIRA");
                m.NotNullable(true);
            });
            ManyToOne(x => x.Evento, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_EVENTO");
                m.NotNullable(true);
            });

            Property(x => x.NomeCracha, m =>
            {
                m.Access(Accessor.Property);
                m.Column("NOME_CRACHA");
                m.Length(150);
                m.NotNullable(false);
            });
            ManyToOne(x => x.Pessoa, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_PESSOA");
                m.NotNullable(true);
                m.Cascade(Cascade.All | Cascade.DeleteOrphans);
            });
            Property(x => x.PrimeiroEncontro, m =>
            {
                m.Access(Accessor.Property);
                m.Column("PRIMEIRO_ENCONTRO");
                m.NotNullable(true);
            });
            Property(x => x.Situacao, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("SITUACAO");
                m.NotNullable(true);
                m.Type<EnumGeneric<EnumSituacaoInscricao>>();
            });            
        }
    }

    public class InscricaoInfantilMapping: SubclassMapping<InscricaoInfantil>
    {
        public InscricaoInfantilMapping()
        {
            DiscriminatorValue("INFANTIL");

            ManyToOne(x => x.InscricaoResponsavel1, m =>
            {
                m.Column("ID_INSC_RESPONSAVEL_1");
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
            });

            ManyToOne(x => x.InscricaoResponsavel2, m =>
            {
                m.Column("ID_INSC_RESPONSAVEL_2");
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
            });
        }
    }

    public class InscricaoParticipanteTrabalhadorMapping: SubclassMapping<InscricaoParticipante>
    {
        public InscricaoParticipanteTrabalhadorMapping()
        {
            DiscriminatorValue("PART_TRAB");

            Bag(x => x.Atividades, m =>
              {
                  m.Cascade(Cascade.All | Cascade.DeleteOrphans);
                  m.Inverse(true);
                  m.Lazy(CollectionLazy.Lazy);
                  m.Access(Accessor.NoSetter);
                  m.Key(k => k.Column("ID_ATIVIDADE"));
              }, c => c.OneToMany(o => o.Class(typeof(AAtividadeInscricao))));

            Property(x => x.Tipo, m =>
            {
                m.Column("TIPO");
                m.Access(Accessor.Property);
                m.NotNullable(false);
                m.Type<EnumGeneric<EnumTipoParticipante>>();
            });
            Property(x => x.InstituicoesEspiritasFrequenta, m =>
            {
                m.Access(Accessor.Property);
                m.Column("INSTITUICOES_ESPIRITAS_FREQ");
                m.NotNullable(false);
            });
            Property(x => x.NomeResponsavelCentro, m =>
            {
                m.Access(Accessor.Property);
                m.Column("NOME_RESP_CENTRO");
                m.Length(150);
                m.NotNullable(false);
            });
            Property(x => x.NomeResponsavelLegal, m =>
            {
                m.Access(Accessor.Property);
                m.Column("NOME_RESP_LEGAL");
                m.Length(150);
                m.NotNullable(false);
            });
            Property(x => x.Observacoes, m =>
            {
                m.Access(Accessor.Property);
                m.Column("OBSERVACOES");
                m.NotNullable(false);
                m.Type(NHibernateUtil.StringClob);
            });
            Property(x => x.TelefoneResponsavelCentro, m =>
            {
                m.Access(Accessor.Property);
                m.Column("TELEFONE_RESP_CENTRO");
                m.Length(15);
                m.NotNullable(false);
            });
            Property(x => x.TelefoneResponsavelLegal, m =>
            {
                m.Access(Accessor.Property);
                m.Column("TELEFONE_RESP_LEGAL");
                m.NotNullable(false);
                m.Length(15);
            });
            Property(x => x.TempoEspirita, m =>
            {
                m.Access(Accessor.Property);
                m.Column("TEMPO_ESPIRITA");
                m.NotNullable(false);
            });
            Component(x => x.Pagamento, m =>
            {
                m.Bag(y => y.Comprovantes, n =>
                {
                    n.Cascade(Cascade.All | Cascade.DeleteOrphans);
                    n.Inverse(true);
                    n.Lazy(CollectionLazy.Lazy);
                    n.Access(Accessor.NoSetter);
                    n.Table("PAGAMENTO_INSCRICAO_COMPROVANTES");
                    n.Key(k => k.Column("ID_ARQUIVO"));
                }, c => c.ManyToMany(o => o.Column("ID_INSCRICAO")));

                m.Property(y => y.Forma, n =>
                  {
                      n.Column("FORMA_PAGAMENTO");
                      n.Access(Accessor.NoSetter);
                      n.NotNullable(false);
                      n.Type<EnumGeneric<EnumPagamento>>();
                  });
                m.Property(y => y.Observacao, n =>
                {
                    n.Column("OBS_PAGAMENTO");
                    n.Access(Accessor.NoSetter);
                    n.NotNullable(false);
                    n.Type(NHibernateUtil.StringClob);
                });
            });

        }
    }
}
