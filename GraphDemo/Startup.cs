using GraphDemo.Interfaces;
using GraphDemo.Query;
using GraphDemo.Repositories;
using GraphDemo.Resolver;
using GraphQL.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GraphDemo
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

            services.Configure<RepositoryConfiguration>(Configuration.GetSection("Cosmos"));

            services
                .AddSingleton<IGraphRepository, GraphRepository>()
                .AddSingleton<IAirportResolver, AirportResolver>()
                .AddSingleton<IStaffResolver, StaffResolver>()
                .AddSingleton<Schema>()
                .AddGraphQL()
                .AddSystemTextJson() // For .NET Core 3+
                .AddGraphTypes();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGraphQL<Schema>("/graphql");

            app.UseGraphiQLServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
