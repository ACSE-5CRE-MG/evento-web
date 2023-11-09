using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace EventoWeb.BancoDados
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Atualização do banco de dados, aguarde....");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var conexaoDados = configuration.GetConnectionString("mysql");

            var servico = new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddMySql5()
                    // Set the connection string
                    .WithGlobalConnectionString(conexaoDados)
                    // Define the assembly containing the migrations
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);

            // Instantiate the runner
            var runner = servico.GetRequiredService<IMigrationRunner>();

            //runner.MigrateDown(4);
            // Execute the migrations
            runner.MigrateUp();

            Console.WriteLine("Atualização realizada com sucesso!!");
            Console.ReadLine();
        }
    }
}
