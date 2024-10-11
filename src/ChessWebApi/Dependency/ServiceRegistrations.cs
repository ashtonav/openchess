namespace ChessWebApi.Dependency;

using System.Text.Json.Serialization;
using Chess.Core.Services;
using Controllers.Examples;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Swashbuckle.AspNetCore.Filters;

public static class ServiceRegistrations
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add Dependency Injection
        builder.Services.AddScoped<IChessService, ChessService>();

        // Configure lowercase URLs
        builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        // Add controllers
        builder.Services.AddControllers()
            .AddJsonOptions(x =>
            {
                // Convert enums to strings when returning response
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddEndpointsApiExplorer();

        // Add swagger and swagger examples
        builder.Services.AddSwaggerGen(c =>
        {
            c.ExampleFilters();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "OpenChess API", Version = "1" });
        });
        builder.Services.AddSwaggerExamplesFromAssemblyOf<MoveExamples>();

        return builder;
    }

    // Enhance logs by adding OpenTelemetry traces.
    // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-with-otel#5-configure-opentelemetry-with-the-correct-providers
    public static WebApplicationBuilder AddOpenTelemetryLogs(this WebApplicationBuilder builder)
    {
        var otel = builder.Services.AddOpenTelemetry();

        // Configure OpenTelemetry Resources with the application name
        otel.ConfigureResource(resource => resource
            .AddService(builder.Environment.ApplicationName));

        // Add Tracing for ASP.NET Core and export to Console
        otel.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();
            tracing.AddConsoleExporter();
        });

        return builder;
    }

    public static IApplicationBuilder AllowAllCors(this IApplicationBuilder builder) =>
        builder.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}
