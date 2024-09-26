using Asp.Versioning;
using Steeltoe.Discovery.Client;
using System.Reflection;
using TestTaskKaspelan.Saga.Filters;
using TestTaskKaspelan.Saga.Services;

namespace TestTaskKaspelan.Saga
{
    /// <summary>
    /// Saga microservice main class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add service discovery.
            builder.Services.AddDiscoveryClient(builder.Configuration);

            // Add API version.
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader());
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddControllers((options) => {
                options.Filters.Add<AppExceptionFilter>();
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(setupAction =>
            {
                var xmlDocumentationFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlDocumentationFullPath = Path.Combine(AppContext.BaseDirectory, xmlDocumentationFile);
                setupAction.IncludeXmlComments(xmlDocumentationFullPath, true);
            });

            // Add services.
            builder.Services.AddServices();

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
