using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EventoWeb.BancoDados.Migracoes
{
    [Migration(03)]
    public class Migracao03 : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            //CriarContratoInscricao();
            AtualizarCodigoInscricao();
        }        

        private void CriarContratoInscricao()
        {
            Create
                .Table("CONTRATOS_INSCRICAO")
                .WithColumn("ID_CONTRATO_INSCRICAO").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("INSTRUCOES_PAGAMENTO").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("PASSA_A_PASSO_INSCRICAO").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("REGULAMENTO").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                    .ForeignKey("FK_CI_EVENTO", "EVENTOS", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade);
        }

        private void AtualizarCodigoInscricao()
        {
            Alter
                .Table("CODIGOS_ACESSO_INSCRICAO")
                .AddColumn("IDENTIFICACAO").AsString(100).Nullable()
                .AlterColumn("ID_INSCRICAO").AsInt32().Nullable();
        }
    }
}
