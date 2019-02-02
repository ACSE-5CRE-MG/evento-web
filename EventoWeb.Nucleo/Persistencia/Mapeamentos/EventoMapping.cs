﻿using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class EventoMapping: ClassMapping<Evento>
    {
        public EventoMapping()
        {
            this.Table("EVENTOS");
            this.Id(x => x.Id, m =>
              {
                  m.Access(NHibernate.Mapping.ByCode.Accessor.NoSetter);
                  m.Column("ID_EVENTO");
                  m.Generator(Generators.Native, g =>
                  {
                      g.Params(new { sequence = "GEN_EVENTOS" });
                  });
              });

            this.Property(x => x.Nome, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Length(250);
                m.Column("NOME");
            });

            this.Component(x => x.PeriodoInscricaoOnLine, c =>
             {
                 c.Access(Accessor.NoSetter);
                 c.Property(o => o.DataInicial, m => {
                     m.Access(Accessor.Property);
                     m.NotNullable(true);
                     m.Column("DATA_INICIO_INSC");
                 });
                 c.Property(o => o.DataFinal, m => {
                     m.Access(Accessor.Property);
                     m.NotNullable(true);
                     m.Column("DATA_FIM_INSC");
                 });
             });

            this.Component(x => x.PeriodoRealizacaoEvento, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.DataInicial, m => {
                    m.Access(Accessor.Property);
                    m.NotNullable(true);
                    m.Column("DATA_INICIO_EVENTO");
                });
                c.Property(o => o.DataFinal, m => {
                    m.Access(Accessor.Property);
                    m.NotNullable(true);
                    m.Column("DATA_FIM_EVENTO");
                });
            });

            this.Property(x => x.DataRegistro, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("DATA_REGISTRO");
            });
            this.Property(x => x.Logotipo, m => {
                m.Access(Accessor.Property);
                m.NotNullable(false);
                m.Column("LOGOTIPO");
                m.Type(NHibernateUtil.StringClob);
            });
            this.Property(x => x.TemDepartamentalizacao, m => {
                m.Access(Accessor.Property);
                m.NotNullable(true);
                m.Column("TEM_DEPARTAMENTALIZACAO");
            });

            this.Component(x => x.ConfiguracaoSalaEstudo, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.ModeloDivisao, m => {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("MODELO_DIV_SL_ESTUDO");
                });
            });

            this.Property(x => x.TemOficinas, m => {
                m.Access(Accessor.Property);
                m.NotNullable(true);
                m.Column("TEM_OFICINAS");
            });
            this.Property(x => x.TemDormitorios, m => {
                m.Access(Accessor.Property);
                m.NotNullable(true);
                m.Column("TEM_DORMITORIOS");
            });

            this.Component(x => x.ConfiguracaoEvangelizacao, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.Publico, m => {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("PUBLICO_EVANGELIZACAO");
                });
            });

            this.Component(x => x.ConfiguracaoSarau, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.TempoDuracaoMin, m => {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("TEMPO_SARAU_MIN");
                });
            });            
        }
    }
}
