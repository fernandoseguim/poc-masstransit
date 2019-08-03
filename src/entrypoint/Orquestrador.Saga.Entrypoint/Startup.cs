using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orquestrador.Saga.Entrypoint.Extensions.MassTransit;
using Orquestrador.Saga.Entrypoint.Extensions.Swagger;
using Orquestrador.Saga.Entrypoint.Extensions.Versioning;
using Orquestrador.Saga.Entrypoint.Publishers;

namespace Orquestrador.Saga.Entrypoint
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddVersioning();
            services.AddSwaggerDocumentation();
            services.AddMassTransitWithRabbitMq(Configuration);
            services.AddScoped<IBankDepositTransactionPublisher, BankDepositTransactionPublisher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider versionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(versionDescriptionProvider);
            app.UseMvc();
        }
    }
}
