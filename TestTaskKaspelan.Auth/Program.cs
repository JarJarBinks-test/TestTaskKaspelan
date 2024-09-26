using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.ConfigServer;
using TestTaskKaspelan.Auth.Services;
using TestTaskKaspelan.Common.Contracts;

namespace TestTaskKaspelan.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add service discovery.
            builder.Services.AddDiscoveryClient(builder.Configuration);

            builder.Services.AddServices();

            builder.Services.AddOptions();
            // Add external config server.
            builder.AddConfigServer();
            // Configure auth options from the configuration
            builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection(AuthOptions.PREFIX));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
