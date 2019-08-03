using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;

namespace Orquestrador.Saga.Entrypoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) =>
                {
                    configuration.Enrich.FromLogContext();
                    configuration.Enrich.WithExceptionDetails();
                    configuration.Enrich.WithMachineName();
                    configuration.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning);
                    configuration.MinimumLevel.Override("MassTransit", Serilog.Events.LogEventLevel.Debug);
                    configuration.WriteTo.Console();
                })
                .UseStartup<Startup>();
    }
}
