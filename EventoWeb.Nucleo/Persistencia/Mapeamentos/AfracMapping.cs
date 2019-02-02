﻿using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    class AfracMapping: ClassMapping<Afrac>
    {
        public AfracMapping()
        {
            this.Table("AFRACS");
            this.Id(x => x.Id, m =>
            {
                m.Access(NHibernate.Mapping.ByCode.Accessor.NoSetter);
                m.Column("ID_AFRAC");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_AFRACS" });
                });
            });

            this.Property(x => x.DeveSerParNumeroTotalParticipantes, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("PAR_TOTAL_PARTICIPANTES");
                m.NotNullable(true);
            });

            this.ManyToOne(x => x.Evento, m =>
            {
                m.NotNullable(true);
                m.Access(Accessor.NoSetter);
                m.Column("ID_EVENTO");
            });

            this.Property(x => x.Nome, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("NOME");
                m.Length(250);
            });

            this.Property(x => x.NumeroTotalParticipantes, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
                m.Column("NUM_MAX_PARTICIPANTES");
            });

            /* this.Bag(x => x.Participantes, m =>
              {
                  m.Cascade(Cascade.All | Cascade.DeleteOrphans);
                  m.Inverse(true);
                  m.Lazy(CollectionLazy.Lazy);
                  m.Access(Accessor.NoSetter);
                  m.Key(k => k.Column("ID_INSCRICAO"));
              }, c=> c.OneToMany(o=> o.Class(typeof(InscricaoParticipante))));*/
        }
    }
}
