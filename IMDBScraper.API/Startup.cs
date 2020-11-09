using IMDBScraper.API.Configs;
using IMDBScraper.DAL.MongoRepository;
using IMDBScraper.DAL.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace IMDBScraper.API
{
    public class Startup
    {
        private string CORS_POLICY = "cors_policy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
       
            // *** init MongoDB *** */
            var mongoSection = Configuration.GetSection("MongoDB");
            var mongoDBConfig = new MongoDBConfig();
            mongoSection.Bind(mongoDBConfig);

            services.AddTransient<IActorRepository>(provider => new ActorRepository(mongoDBConfig));

            /* *** [init Swagger] *** */
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IMDB Scraper API", Version = "v1" });
            });

            /* *** [ELSE] *** */
            services.AddControllers();

            var corsSection = Configuration.GetSection("CORS");
            var corsConfig = new CORSConfig();
            corsSection.Bind(corsConfig);

            services.AddCors(options =>
            {
                options.AddPolicy(name: CORS_POLICY,
                                  builder =>
                                  {
                                      builder.WithOrigins(corsConfig.FrontEndURL)
                                        .AllowAnyMethod()
                                        .AllowAnyHeader()
                                        .AllowCredentials();
                                  });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            app.UseExceptionHandler("/error"); // Add this
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My First Swagger");
            });

        app.UseRouting();
            app.UseCors(CORS_POLICY);
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
