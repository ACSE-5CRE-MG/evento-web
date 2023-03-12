using FluentMigrator;

namespace EventoWeb.BancoDados.Migracoes
{
    [Migration(04)]
    public class Migracao04 : Migration
    {
        public override void Down()
        {
            Alter
                .Table("EVENTOS")
                .AddColumn("TEM_OFICINAS").AsBoolean().NotNullable().WithDefaultValue(true);

            Update
                .Table("EVENTOS")
                .Set(new
                {
                    TEM_OFICINAS = 0
                })
                .Where(new
                {
                    MODELO_DIV_OFICINAS = (int?)null
                });

            Delete.Column("MODELO_DIV_OFICINAS").FromTable("EVENTOS");
            Delete.Column("PERMITE_ESCOLHA_DORMIR_EVENTO").FromTable("EVENTOS");
        }

        public override void Up()
        {
            Alter
                .Table("EVENTOS")
                .AddColumn("MODELO_DIV_OFICINAS").AsInt16().Nullable()
                .AddColumn("PERMITE_ESCOLHA_DORMIR_EVENTO").AsBoolean().Nullable();

            Update
                .Table("EVENTOS")
                .Set(new
                {
                    MODELO_DIV_OFICINAS = 0
                })
                .Where(new
                {
                    TEM_OFICINAS = 1
                });

            Update
                .Table("EVENTOS")
                .Set(new
                {
                    PERMITE_ESCOLHA_DORMIR_EVENTO = 0
                })
                .Where(new
                {
                    TEM_DORMITORIOS = 1
                });

            Delete.Column("TEM_OFICINAS").FromTable("EVENTOS");
        }        
    }
}
