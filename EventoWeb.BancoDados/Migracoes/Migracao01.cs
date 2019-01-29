using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EventoWeb.BancoDados.Migracoes
{
    [Migration(01)]
    public class Migracao01 : Migration
    {
        public override void Down()
        {
            Delete.Table("EVENTOS");
            Delete.Table("USUARIOS");
        }

        public override void Up()
        {
            CriarTabelaEventos();
            CriarTabelaUsuarios();
            CriarTabelaSalasEstudo();
            CriarTabelaAfracs();
            CriarTabelaDepartamentos();
            CriarTabelaConfiguracaoEmail();
        }

        private void CriarTabelaEventos()
        {
            Create.Table("EVENTOS")
                .WithColumn("ID_EVENTO").AsInt32().PrimaryKey().Identity()
                .WithColumn("NOME").AsString(100).NotNullable()
                .WithColumn("DATA_INICIO_INSC").AsDateTime().NotNullable()
                .WithColumn("DATA_FIM_INSC").AsDateTime().NotNullable()
                .WithColumn("DATA_INICIO_EVENTO").AsDateTime().NotNullable()
                .WithColumn("DATA_FIM_EVENTO").AsDateTime().NotNullable()
                .WithColumn("DATA_REGISTRO").AsDateTime().NotNullable()
                .WithColumn("LOGOTIPO").AsString(Int32.MaxValue)
                .WithColumn("TEM_DEPARTAMENTALIZACAO").AsInt16().NotNullable()
                .WithColumn("MODELO_DIV_SL_ESTUDO").AsInt16().NotNullable()
                .WithColumn("TEM_OFICINAS").AsInt16().NotNullable()
                .WithColumn("TEM_DORMITORIOS").AsInt16().NotNullable()
                .WithColumn("PUBLICO_EVANGELIZACAO").AsInt16()
                .WithColumn("TEMPO_SARAU_MIN").AsInt32().Nullable();
        }

        private void CriarTabelaUsuarios()
        {
            Create.Table("USUARIOS")
                .WithColumn("LOGIN").AsString(150).PrimaryKey();

            Insert.IntoTable("USUARIOS").Row(new { LOGIN = "robsonmbobbi@gmail.com" });
        }

        private void CriarTabelaSalasEstudo()
        {
            Create
                .Table("SALAS_ESTUDO")
                    .WithColumn("ID_SALA_ESTUDO").AsInt32().PrimaryKey().Identity()
                    .WithColumn("PAR_TOTAL_PARTICIPANTES").AsInt16().NotNullable().WithDefaultValue(0)
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_EVENTO_SALA", "EVENTOS", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("IDADE_MAX").AsInt32()
                    .WithColumn("IDADE_MIN").AsInt32()
                    .WithColumn("NOME").AsString(250).NotNullable();                
        }

        private void CriarTabelaAfracs()
        {
            Create
                .Table("AFRACS")
                    .WithColumn("ID_AFRAC").AsInt32().PrimaryKey().Identity()
                    .WithColumn("PAR_TOTAL_PARTICIPANTES").AsInt16().NotNullable().WithDefaultValue(0)
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_EVENTO_SALA", "EVENTOS", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("NOME").AsString(250).NotNullable()
                    .WithColumn("NUM_MAX_PARTICIPANTES").AsInt32();
        }

        private void CriarTabelaDepartamentos()
        {
            Create
                .Table("DEPARTAMENTOS")
                    .WithColumn("ID_DEPARTAMENTO").AsInt32().PrimaryKey().Identity()
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_EVENTO_SALA", "EVENTOS", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("NOME").AsString(250).NotNullable();
        }

        private void CriarTabelaConfiguracaoEmail()
        {
            Create
                .Table("CONFIGURACOES_EMAIL")
                    .WithColumn("ID_CONFIGURACAO_EMAIL").AsInt32().PrimaryKey().Identity()
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_EVENTO_SALA", "EVENTOS", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("ENDERECO_EMAIL").AsString(150).Nullable()
                    .WithColumn("USUARIO_EMAIL").AsString(100).Nullable()
                    .WithColumn("SENHA_EMAIL").AsString(100).Nullable()
                    .WithColumn("SERVIDOR_EMAIL").AsString(50).Nullable()
                    .WithColumn("PORTA_SERVIDOR").AsInt32().Nullable()
                    .WithColumn("TIPO_SEGURANCA").AsInt16().Nullable()
                    .WithColumn("ASSUNTO_INSC_CONFIRMADA").AsString(150).Nullable()
                    .WithColumn("MENSAGEM_INSC_CONFIRMADA").AsString(Int32.MaxValue).Nullable()
                    .WithColumn("ASSUNTO_INSC_REGISTRADA").AsString(150).Nullable()
                    .WithColumn("MENSAGEM_INSC_REGISTRADA").AsString(Int32.MaxValue).Nullable();
        }
    }
}
