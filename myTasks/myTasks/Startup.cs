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
            services.AddSingleton<ITaskRepository>(new InMemoryTaskRepository());
            services.AddMvc().AddFluentValidation();

            services.AddTransient<IValidator<TaskItem>, TaskItemValidator>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}
