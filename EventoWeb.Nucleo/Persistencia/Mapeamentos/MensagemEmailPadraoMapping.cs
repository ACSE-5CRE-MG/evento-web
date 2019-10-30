using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    class MensagemEmailPadraoMapping: ClassMapping<MensagemEmailPadrao>
    {
        public MensagemEmailPadraoMapping()
        {
            this.Table("MENSAGEM_EMAIL_PADRAO");
            this.Id(x => x.Id, m =>
            {
                m.Access(NHibernate.Mapping.ByCode.Accessor.NoSetter);
                m.Column("ID_MENSAGEM_EMAIL_PADRAO");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_MENSAGEM_EMAIL_PADRAO" });
                });
            });

            this.Component(x => x.MensagemInscricaoConfirmada, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.Assunto, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("ASSUNTO_INSC_CONFIRMADA");
                    m.Length(150);
                });
                c.Property(O => O.Mensagem, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("MENSAGEM_INSC_CONFIRMADA");
                    m.Type(NHibernateUtil.StringClob);
                });
            });

            this.Component(x => x.MensagemInscricaoRegistrada, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.Assunto, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("ASSUNTO_INSC_REGISTRADA");
                    m.Length(150);
                });
                c.Property(O => O.Mensagem, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("MENSAGEM_INSC_REGISTRADA");
                    m.Type(NHibernateUtil.StringClob);
                });
            });

            this.Component(x => x.MensagemInscricaoCodigoAcessoAcompanhamento, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.Assunto, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("ASSUNTO_INSC_COD_ACESSO_ACOMP");
                    m.Length(150);
                });
                c.Property(O => O.Mensagem, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("MENSAGEM_INSC_COD_ACESSO_ACOMP");
                    m.Type(NHibernateUtil.StringClob);
                });
            });

            this.Component(x => x.MensagemInscricaoCodigoAcessoCriacao, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.Assunto, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("ASSUNTO_INSC_COD_ACESSO_CRI");
                    m.Length(150);
                });
                c.Property(O => O.Mensagem, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("MENSAGEM_INSC_COD_ACESSO_CRI");
                    m.Type(NHibernateUtil.StringClob);
                });
            });
        }
    }
}
