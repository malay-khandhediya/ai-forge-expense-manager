using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using CorrelationId;
using FastEndpoints;
using Peddle.Foundation.Common.Extensions;
using Peddle.Foundation.Common.Extensions.Configuration;
using Peddle.Foundation.Common.Extensions.Configuration.Json;
using Peddle.Foundation.Common.Extensions.Configuration.SystemsManager;
using Peddle.Foundation.Common.Logging.Dependencies;
using Peddle.Foundation.Common.Middleware.Errors;
using Peddle.ParameterStore.Shared;
using Serilog;

var webAppBuilder = WebApplication.CreateBuilder(args);


webAppBuilder.Services.AddEndpointsApiExplorer();

webAppBuilder.Configuration.SetBasePath(webAppBuilder.Environment.ContentRootPath).AddEnvironmentVariables();

webAppBuilder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
webAppBuilder.Configuration.AddSystemsManagerForProduct(Product.Universal);

var peddleLoggingSection = webAppBuilder.Configuration.GetRequiredSection("Peddle:Logging");

var loggerSystemsManagerPaths = peddleLoggingSection
    .GetRequiredSection("SystemsManagerPaths")
    .Get<List<SystemsManagerPath>>();

var loggerConfigPaths = peddleLoggingSection
    .GetRequiredSection("JsonConfigPaths")
    .Get<List<JsonParameterPath>>();

webAppBuilder.Configuration.AddSystemsManagerForPaths(loggerSystemsManagerPaths!, DependenciesVersion.GetVersionString())
                .AddJsonParameters(loggerConfigPaths!);

webAppBuilder.Logging.ClearProviders();
webAppBuilder.Host.UseSerilog((context, _, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration).EnableSensitiveDataMasking());

webAppBuilder.Services.AddControllers();

webAppBuilder.Services.AddConfigurations(webAppBuilder.Configuration)
    .AddServiceExtension(webAppBuilder.Configuration)
    .AddAuthentication(webAppBuilder.Configuration);
var app = webAppBuilder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCorrelationId();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseFastEndpoints(c =>
{
    c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    c.Serializer.Options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    c.Serializer.Options.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
    c.Endpoints.Configurator = ep =>
    {
        ep.PostProcessors(Order.After, new GlobalErrorLogger());
    };
    c.Versioning.Prefix = "v";
    c.Versioning.DefaultVersion = 1;
    c.Versioning.PrependToRoute = true;
    c.Binding.Modifier = SnakeCaseQueryBinder.SnakeCaseBindFunc;

});
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapLivenessHealthChecks("parameter-store/api/v1/health/liveness");
app.MapReadinessHealthChecks("parameter-store/api/v1/health/readiness");

app.Run();