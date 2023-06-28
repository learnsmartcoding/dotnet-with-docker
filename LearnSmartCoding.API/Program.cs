
using LearnSmartCoding.Api.Domain;
using LearnSmartCoding.Api.Interfaces;
using LearnSmartCoding.Api.Middleware;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace LearnSmartCoding.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var name = typeof(Program).Assembly.GetName().Name;

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .Enrich.FromLogContext()
               .Enrich.WithMachineName()
               .Enrich.WithProperty("Assembly", name ?? "")
                // available sinks: https://github.com/serilog/serilog/wiki/Provided-Sinks
                // Seq: https://datalust.co/seq
                // Seq with Docker: https://docs.datalust.co/docs/getting-started-with-docker
                //.WriteTo.Seq(serverUrl: "http://host.docker.internal:5341")
                .WriteTo.Seq(serverUrl: "http://seq_in_dc:5341")
               .WriteTo.Console()
               .CreateLogger();

            try
            {
                Log.Information("Starting web application");

                // http://bit.ly/aspnet-builder-defaults
                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog(); //Add this line
                                           // Add services to the container.

                //Adding a vault to configuration layers
                //builder.Host.ConfigureAppConfiguration((context, builder) =>
                //{
                //    //builder. //Add your vault provider here
                //});

                var config = builder.Configuration;

                var connectionString = config.GetConnectionString("DbContext");
                var simpleProperty = config.GetValue<string>("SimpleProperty");
                var nestedProp = config.GetValue<string>("Inventory:NestedProperty");


               Log.ForContext("ConnectionString", connectionString)
                    .ForContext("SimpleProperty", simpleProperty)
                    .ForContext("Inventory:NestedProperty", nestedProp)
                    .Information("Loaded configuration!", connectionString);

                var dbgView = config.GetDebugView();
                Log.ForContext("ConfigurationDebug", dbgView)
                    .Information("Configuration dump.");

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddScoped<IProductLogic, ProductLogic>();
                builder.Services.AddScoped<IQuickOrderLogic, QuickOrderLogic>();

                var app = builder.Build();

                app.UseMiddleware<CustomExceptionHandlingMiddleware>();

                // Configure the HTTP request pipeline.
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCustomRequestLogging();
                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
