using FluentMigrator;
using System;
using System.Collections.Generic;
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
            Create.Table("EVENTOS")
                .WithColumn("ID_EVENTO").AsInt32().PrimaryKey().Identity()
                .WithColumn("NOME").AsString(100).NotNullable()
                .WithColumn("DATA_INICIO_INSC").AsDateTime().NotNullable()
                .WithColumn("DATA_FIM_INSC").AsDateTime().NotNullable()
                .WithColumn("DATA_INICIO_EVENTO").AsDateTime().NotNullable()
                .WithColumn("DATA_FIM_EVENTO").AsDateTime().NotNullable()
                .WithColumn("DATA_REGISTRO").AsDateTime().NotNullable()
                .WithColumn("SITUACAO").AsInt16().NotNullable()
                .WithColumn("LOGOTIPO").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("TEM_DEPARTAMENTALIZACAO").AsInt16().NotNullable()
                .WithColumn("TEM_SALAS_ESTUDO").AsInt16().NotNullable()
                .WithColumn("MODELO_DIV_SL_ESTUDO").AsInt16().NotNullable()
                .WithColumn("TEM_OFICINAS").AsInt16().NotNullable()
                .WithColumn("TEM_DORMITORIOS").AsInt16().NotNullable()
                .WithColumn("TEM_EVANGELIZACAO").AsInt16().NotNullable()
                .WithColumn("PUBLICO_EVANGELIZACAO").AsInt16().NotNullable()
                .WithColumn("TEM_SARAU").AsInt16().NotNullable()
                .WithColumn("TEMPO_SARAU_MIN").AsInt32().Nullable()
                .WithColumn("ENDERECO_EMAIL").AsString(150).Nullable()
                .WithColumn("USUARIO_EMAIL").AsString(100).Nullable()
                .WithColumn("SENHA_EMAIL").AsString(100).Nullable()
                .WithColumn("SERVIDOR_EMAIL").AsString(50).Nullable()
                .WithColumn("PORTA_SERVIDOR").AsInt32().Nullable()
                .WithColumn("TIPO_SEGURANCA").AsInt16().Nullable()
                .WithColumn("TITULO_EMAIL").AsString(150).Nullable()
                .WithColumn("MENSAGEM_EMAIL").AsString(Int32.MaxValue).Nullable();

            Create.Table("USUARIOS")
                .WithColumn("LOGIN").AsString(150).PrimaryKey();

            Insert.IntoTable("USUARIOS").Row(new { LOGIN = "robsonmbobbi@gmail.com" });
        }
    }
}
