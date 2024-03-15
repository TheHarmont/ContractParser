using ContractParser.Domain.Abstract;
using ContractParser.Domain.DAL;
using ContractParser.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;

namespace ContractParser
{
    public class Program
    {
        static void Main()
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

                var servicesProvider = BuildDi(config);

                var app = servicesProvider.GetRequiredService<ContractParserService>();
                app.Application();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Программа остановлена из-за исключения");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        private static IServiceProvider BuildDi(IConfiguration config)
        {
            return new ServiceCollection()
               .AddDbContext<DataBaseContext>(options => options.UseSqlServer(config.GetConnectionString("RegisterOfContracts")))
               .AddTransient<IContractRepository, EFContractRepository>()
               .AddTransient<ContractParserService>() 
               .AddLogging(loggingBuilder =>
               {
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.None);
                   loggingBuilder.AddNLog(config);
               })
               .BuildServiceProvider();
        }
    }
}
