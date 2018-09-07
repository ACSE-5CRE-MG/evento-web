using EventoWeb.Nucleo.Negocio.Entidades;
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
            this.Property(x => x.DataInicioInscricao, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("DATA_INICIO_INSC");
            });
            this.Property(x => x.DataFimInscricao, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("DATA_FIM_INSC");
            });
            this.Property(x => x.DataInicioEvento, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("DATA_INICIO_EVENTO");
            });
            this.Property(x => x.DataFimEvento, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("DATA_FIM_EVENTO");
            });
            this.Property(x => x.DataRegistro, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("DATA_REGISTRO");
            });
            this.Property(x => x.Situacao, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("SITUACAO");
                m.Type<EnumGeneric<SituacaoEvento>>();
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
            this.Property(x => x.TemSalasEstudo, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("TEM_SALAS_ESTUDO");
            });
            this.Property(x => x.ModeloDivisaoSalasEstudo, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
                m.Column("MODELO_DIV_SL_ESTUDO");
                m.Type<EnumGeneric<EnumModeloDivisaoSalasEstudo>>();
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
            this.Property(x => x.TemEvangelizacao, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("TEM_EVANGELIZACAO");
            });
            this.Property(x => x.PublicoEvangelizacao, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
                m.Column("PUBLICO_EVANGELIZACAO");
                m.Type<EnumGeneric<EnumPublicoEvangelizacao>>();
            });
            this.Property(x => x.TemSarau, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("TEM_SARAU");
            });
            this.Property(x => x.TempoDuracaoSarauMin, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
                m.Column("TEMPO_SARAU_MIN");
            });

            this.Component(x => x.ConfiguracaoEmail, c =>
              {
                  c.Access(Accessor.NoSetter);

                  c.Property(x => x.EnderecoEmail, m => {
                      m.Access(Accessor.NoSetter);
                      m.NotNullable(false);
                      m.Column("ENDERECO_EMAIL");
                      m.Length(150);
                  });
                  c.Property(x => x.UsuarioEmail, m => {
                      m.Access(Accessor.NoSetter);
                      m.NotNullable(false);
                      m.Column("USUARIO_EMAIL");
                      m.Length(100);
                  });
                  c.Property(x => x.SenhaEmail, m => {
                      m.Access(Accessor.NoSetter);
                      m.NotNullable(false);
                      m.Column("SENHA_EMAIL");
                      m.Length(100);
                  });
                  c.Property(x => x.ServidorEmail, m => {
                      m.Access(Accessor.NoSetter);
                      m.NotNullable(false);
                      m.Column("SERVIDOR_EMAIL");
                      m.Length(50);
                  });
                  c.Property(x => x.PortaServidor, m => {
                      m.Access(Accessor.NoSetter);
                      m.NotNullable(false);
                      m.Column("PORTA_SERVIDOR");
                  });
                  c.Property(x => x.TipoSeguranca, m => {
                      m.Access(Accessor.NoSetter);
                      m.NotNullable(false);
                      m.Column("TIPO_SEGURANCA");
                      m.Type<EnumGeneric<TipoSegurancaEmail>>();
                  });
                  c.Property(x => x.TituloEmailConfirmacaoInscricao, m => {
                      m.Access(Accessor.NoSetter);
                      m.NotNullable(false);
                      m.Column("TITULO_EMAIL");
                      m.Length(150);
                  });
                  c.Property(x => x.MensagemEmailConfirmacaoInscricao, m => {
                      m.Access(Accessor.NoSetter);
                      m.NotNullable(false);
                      m.Column("MENSAGEM_EMAIL");
                      m.Type(NHibernateUtil.StringClob);
                  });
              });
        }
    }
}
