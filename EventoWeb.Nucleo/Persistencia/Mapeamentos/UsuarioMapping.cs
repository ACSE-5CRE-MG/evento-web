using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class UsuarioMapping : ClassMapping<Usuario>
    {
        public UsuarioMapping()
        {
            this.Table("USUARIOS");
            this.Id(x => x.Login, m =>
              {
                  m.Access(Accessor.Property);
                  m.Column("LOGIN");
                  m.Length(150);
                  m.Generator(Generators.Assigned);
              });
        }
    }
}
