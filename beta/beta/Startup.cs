using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using beta.DataAccess;
using beta.BusinessLogic;

namespace beta
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
            //for API
            services.AddControllers();

            //for dependancy injection and IoC
            services.AddScoped<IRepository, Repository>();

            //for injection of the SB-event listener
            services.AddSingleton<IServiceBusConsumer, ServiceBusConsumer>();
            

            //for PostgreSQL
            //get the connection string and from User Secrets and use the string to connect to the postgre database
            services.AddDbContext<DataAccess.Entities.NumDBContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("ElephantDB2")));

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //for the service-bus listener
            //define the event-listener
            var bus = app.ApplicationServices.GetService<IServiceBusConsumer>();

            //start listening
            bus.RegisterOnMessageHandlerAndReceiveMessages();

        }
    }
}
