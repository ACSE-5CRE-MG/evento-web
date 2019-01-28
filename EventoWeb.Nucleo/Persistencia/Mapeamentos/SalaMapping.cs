using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    class SalaMapping: ClassMapping<SalaEstudo>
    {
        public SalaMapping()
        {
            this.Table("SALAS_ESTUDO");
            this.Id(x => x.Id, m =>
            {
                m.Access(NHibernate.Mapping.ByCode.Accessor.NoSetter);
                m.Column("ID_SALA_ESTUDO");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_SALAS_ESTUDO" });
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

            this.Component(x => x.FaixaEtaria, c =>
              {
                  c.Access(Accessor.NoSetter);
                  c.Property(p => p.IdadeMax, pm =>
                   {
                       pm.Access(Accessor.NoSetter);
                       pm.Column("IDADE_MAX");
                       pm.NotNullable(false);
                   });

                  c.Property(p => p.IdadeMin, pm =>
                  {
                      pm.Access(Accessor.NoSetter);
                      pm.Column("IDADE_MIN");
                      pm.NotNullable(false);
                  });
              });

            this.Property(x => x.Nome, m =>
              {
                  m.Access(Accessor.NoSetter);
                  m.NotNullable(true);
                  m.Column("NOME");
                  m.Length(250);
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
