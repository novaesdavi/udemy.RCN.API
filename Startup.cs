using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RCN.API.Data;

namespace RCN.API
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
        
        // services.AddMvc().AddJsonOptions(opt => {opt.JsonSerializerOptions.IgnoreNullValues})
            services.AddControllers()
            .AddJsonOptions(opt =>{
            opt.JsonSerializerOptions.IgnoreNullValues = true;
            })
            .AddXmlSerializerFormatters();
            
            services.AddDbContext<ProdutoContexto>(opt => 
                opt.UseInMemoryDatabase(databaseName:"produtoInMemory")
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            
            services.AddVersionedApiExplorer(opt=>{
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
            });
            
            services.AddApiVersioning();
            services.AddResponseCaching();
            
            services.AddResponseCompression(opt=>
            {
                opt.Providers.Add<BrotliCompressionProvider>();
                opt.EnableForHttps = true;
            });
            
            services.AddSwaggerGen(c =>
            {
            
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            
            foreach (var item in provider.ApiVersionDescriptions)
            {
                c.SwaggerDoc(
                    item.GroupName, new OpenApiInfo { 
                    Title = $"Api de Produtos {item.ApiVersion}", 
                    Version = item.ApiVersion.ToString() 
                });
            }

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseResponseCaching();
            
            app.UseResponseCompression();
            
            app.UseHttpsRedirection();

            app.UseAuthorization();          
            
            app.UseSwagger();
                        
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                foreach (var item in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", item.GroupName);
                    c.RoutePrefix = string.Empty;
                }
                c.RoutePrefix = string.Empty;
            });
      
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
