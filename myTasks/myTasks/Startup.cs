using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using myTasks.Models;
using myTasks.Persistence;

namespace myTasks
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                // OK for testing purposes
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSingleton<ITaskRepository>(new InMemoryTaskRepository());
            services.AddMvc().AddFluentValidation();

            services.AddTransient<IValidator<TaskItemDto>, TaskItemDtoValidator>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("MyPolicy");
            app.UseMvc();
        }
    }
}
