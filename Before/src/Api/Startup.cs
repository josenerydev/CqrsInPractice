using Api.Utils;
using Logic.Students;
using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(new SessionFactory(Configuration["ConnectionString"]));
            services.AddTransient<UnitOfWork>();
            services.AddTransient<ICommandHandler<EditPersonalInfoCommand>, EditPersonalInfoCommandHandler>();
            services.AddSingleton<Messages>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();
        }
    }
}
