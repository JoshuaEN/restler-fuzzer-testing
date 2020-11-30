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
using Server.Authentication;

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
#if JSON_PASCAL_CASE
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // Pascal Case
#else
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
#endif
            });
            services.AddSwaggerGen(c => {
                var filePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "Server.xml");
                c.IncludeXmlComments(filePath);

                c.OperationFilter<RestlerOperationFilter>();
            });
            services.AddLogging();

#if ENDPOINT_AUTHN
            services.AddAuthentication(c =>
            {
                c.AddScheme<TimestampAuthenticationHandler>(TimestampAuthenticationHandler.AuthenticationScheme, TimestampAuthenticationHandler.AuthenticationScheme);
                c.DefaultScheme = TimestampAuthenticationHandler.AuthenticationScheme;
            });

            services.AddSingleton<InMemoryStorageService<AuthRequiredResourceController, string, AuthRequiredResource>>();
#endif

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
#if ENDPOINT_AS_EXPECT_PASCAL_CASES
            services.AddSingleton<InMemoryStorageService<AsExpectPascalCaseController, string, AsExpectPascalCase>>();
#endif
#if ENDPOINT_NESTED
            services.AddSingleton<InMemoryStorageService<NestedResourceController, (string asExpectId, string nestedResourceId), NestedResource>>();
            services.AddSingleton<InMemoryStorageService<NestedResourceAdjacentController, (string asExpectId, string nestedResourceAdjacentId), NestedResourceAdjacent>>();
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
