using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Server.Controllers;
using Server.Models;
using Server.Services;
using Server.OpenAPIFilters;

namespace Server
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
            services.AddControllers();
            services.AddMvc().AddJsonOptions(options => {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
            services.AddSwaggerGen(c => {
                c.OperationFilter<RestlerOperationFilter>();
            });
            services.AddLogging();

            #if ENDPOINT_POST_ON_RESOURCES
                services.AddSingleton<InMemoryStorageService<PostOnResourceController, string, PostOnResource>>();
            #endif
            #if ENDPOINT_PUT_ONLYS
                services.AddSingleton<InMemoryStorageService<PutOnlyController, string, PutOnly>>();
            #endif
            #if ENDPOINT_TRADITIONALS
                services.AddSingleton<InMemoryStorageService<TraditionalController, string, Traditional>>();
            #endif
            #if ENDPOINT_AS_EXPECTED
                services.AddSingleton<InMemoryStorageService<AsExpectController, string, AsExpect>>();
            #endif
            #if ENDPOINT_RECURSIVE_MODELS
                services.AddSingleton<InMemoryStorageService<RecursiveModelController, string, RecursiveModel>>();
            #endif
            #if ENDPOINT_NESTED_MODELS
                services.AddSingleton<InMemoryStorageService<NestedModelController, string, NestedModel>>();
            #endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
