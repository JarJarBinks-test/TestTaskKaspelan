using Asp.Versioning;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Steeltoe.Messaging.RabbitMQ.Config;
using System.Reflection;
using TestTaskKaspelan.Order.Filters;
using TestTaskKaspelan.Order.Services;

namespace TestTaskKaspelan.Order
{
    /// <summary>
    /// Order microservice main class.
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

            builder.Services.AddOptions();
            // Add external config server.
            builder.AddConfigServer();
            // Configure rabbit options from the configuration
            builder.Services.Configure<RabbitOptions>(builder.Configuration.GetSection(RabbitOptions.PREFIX));

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

            // Configure services.
            builder.Services.AddServices(builder.Configuration);

            var app = builder.Build();

            app.Services.SetupServices();
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
