using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.ConfigServer;
using System.Text;
using TestTaskKaspelan.Common.Contracts;

namespace TestTaskKaspelan.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOptions();
            // Add external config server.
            builder.AddConfigServer();
            // Configure auth options from the configuration
            var authConfig = builder.Configuration.GetSection(AuthOptions.PREFIX);
            builder.Services.Configure<AuthOptions>(authConfig);

            //Jwt configuration starts here
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = authConfig.GetValue<string>("issuer"),
                     ValidAudience = authConfig.GetValue<string>("issuer"),
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.GetValue<string>("key")))
                 };
             });

            // Add ocelot.
            builder.Configuration.AddJsonFile("ocelot.json", false, true);
            builder.Services.AddDiscoveryClient(builder.Configuration);
            builder.Services.AddOcelot(builder.Configuration).AddEureka();

            // Add services to the container.
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
            // Enable Ocelot middleware
            app.UseOcelot();

            app.MapControllers();

            app.Run();
        }
    }
}
