using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EventoWeb.BancoDados.Migracoes
{
    [Migration(02)]
    public class Migracao02 : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            CriarConta();
            CriarFaturamento();
            CriarTitulo();
            CriarTransacao();
            CriarSalasEstudoParticipantes();
        }        

        private void CriarConta()
        {
            Create
                .Table("CONTAS")
                .WithColumn("ID_CONTA").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("DESCRICAO").AsString(100).NotNullable()
                .WithColumn("SALDO_INICIAL").AsCurrency().NotNullable()
                .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                    .ForeignKey("FK_CONTA_EVENTO", "EVENTOS", "ID_EVENTO").OnDelete(Rule.None).OnUpdate(Rule.None);
        }

        private void CriarFaturamento()
        {
            Create
                .Table("FATURAMENTOS")
                .WithColumn("ID_FATURAMENTO").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("TIPO_FATURAMENTO").AsString(30).NotNullable()
                .WithColumn("DESCRICAO").AsString(500).NotNullable()
                .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                    .ForeignKey("FK_FAT_EVENTO", "EVENTOS", "ID_EVENTO").OnDelete(Rule.None).OnUpdate(Rule.None)
                .WithColumn("FATURADO").AsBoolean().NotNullable()
                .WithColumn("MOTIVO_DESCONTO").AsString(500).Nullable()
                .WithColumn("TIPO").AsInt16().NotNullable()
                .WithColumn("VALOR_BRUTO").AsCurrency().NotNullable()
                .WithColumn("VALOR_DESCONTO").AsCurrency().NotNullable()
                .WithColumn("DATA").AsDateTime().NotNullable();

            Create
                .Index("IDX_FAT_1")
                .OnTable("FATURAMENTOS")
                .OnColumn("DESCRICAO").Ascending()
                .OnColumn("DATA").Ascending();

            Create
                .Table("FATURAMENTO_INSCRICOES")
                .WithColumn("ID_FATURAMENTO").AsInt32().PrimaryKey().NotNullable()
                    .ForeignKey("FK_FATINSC_FAT", "FATURAMENTOS", "ID_FATURAMENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                .WithColumn("ID_INSCRICAO").AsInt32().PrimaryKey().NotNullable()
                    .ForeignKey("FK_FATINSC_INSC", "INSCRICOES", "ID_INSCRICAO").OnDelete(Rule.None).OnUpdate(Rule.Cascade);
        }

        private void CriarTitulo()
        {
            Create
                .Table("TITULOS_FINANCEIROS")
                .WithColumn("ID_TITULO_FINANCEIRO").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("DATA_CRIACAO").AsDateTime().NotNullable()
                .WithColumn("DATA_VENCIMENTO").AsDate().NotNullable()
                .WithColumn("DESCRICAO").AsString(300).Nullable()
                .WithColumn("TIPO").AsInt16().NotNullable()
                .WithColumn("LIQUIDADO").AsBoolean().NotNullable()
                .WithColumn("VALOR").AsCurrency().NotNullable()
                .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                    .ForeignKey("FK_TIT_EVENTO", "EVENTOS", "ID_EVENTO").OnDelete(Rule.None).OnUpdate(Rule.None)
                .WithColumn("ID_FATURAMENTO").AsInt32().NotNullable()
                    .ForeignKey("FK_TIT_FAT", "FATURAMENTOS", "ID_FATURAMENTO").OnDelete(Rule.None).OnUpdate(Rule.None);

            Create
                .Index("IDX_TIT_1")
                .OnTable("TITULOS_FINANCEIROS")
                .OnColumn("ID_EVENTO").Ascending()
                .OnColumn("DATA_VENCIMENTO").Ascending()
                .OnColumn("DESCRICAO").Ascending()
                .OnColumn("TIPO").Ascending();
        }

        private void CriarTransacao()
        {
            Create
                .Table("TRANSACOES_FINANCEIRAS")
                .WithColumn("ID_TRANSACAO_FINACEIRA").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("DATA_HORA").AsDateTime().NotNullable()
                .WithColumn("O_QUE").AsString(500).NotNullable()
                .WithColumn("ID_CONTA").AsInt32().NotNullable()
                    .ForeignKey("FK_TRA_CONTA", "CONTAS", "ID_CONTA").OnDelete(Rule.None).OnUpdate(Rule.None)
                .WithColumn("TIPO").AsInt16().NotNullable()
                .WithColumn("VALOR").AsCurrency().NotNullable()
                .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                    .ForeignKey("FK_TRA_EVENTO", "EVENTOS", "ID_EVENTO").OnDelete(Rule.None).OnUpdate(Rule.None)
                .WithColumn("ID_ORIGEM").AsInt32().NotNullable();

            Create
                .Index("IDX_TRA_1")
                .OnTable("TRANSACOES_FINANCEIRAS")
                .OnColumn("ID_EVENTO").Ascending()
                .OnColumn("ID_CONTA").Ascending()
                .OnColumn("DATA_HORA").Ascending();

            Create
                .Index("IDX_TRA_2")
                .OnTable("TRANSACOES_FINANCEIRAS")
                .OnColumn("ID_CONTA").Ascending()
                .OnColumn("DATA_HORA").Ascending();
        }

        private void CriarSalasEstudoParticipantes()
        {
            Create
                .Table("SALAS_ESTUDO_ESCOLHIDAS")
                .WithColumn("ID_SALA_ESTUDO").AsInt32().PrimaryKey().NotNullable()
                    .ForeignKey("FK_SEE_SALA", "SALAS_ESTUDO", "ID_SALA_ESTUDO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                .WithColumn("ID_INSCRICAO").AsInt32().PrimaryKey().NotNullable()
                    .ForeignKey("FK_SEE_INSC", "INSCRICOES", "ID_INSCRICAO").OnDelete(Rule.None).OnUpdate(Rule.Cascade);
        }
    }
}
