using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Peddle.Foundation.Common.Constants;
using Peddle.Foundation.Common.Filters;
using Peddle.Foundation.Common.JsonCasing;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;
using Peddle.ParameterStore.Domain;
using Peddle.ParameterStore.Shared.CacheIntegration;

namespace Peddle.ParameterStore.Shared
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddServiceExtension(this IServiceCollection services,
            IConfiguration configuration)
        {
            var mapperConfig = new MapperConfiguration(mc => { mc.AddMaps(Assembly.GetExecutingAssembly()); });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.RegisterCorrelationIdServices();
            services.RegisterCaching(configuration);
            services.RegisterInternalServices();
            services.AddSingleton<Peddle.Foundation.Common.Middleware.Errors.IErrorResponsesProvider,
                Peddle.Foundation.Common.Middleware.Errors.ErrorResponsesProvider>();
            services.RegisterHealthCheck();
            return services;
        }

        public static IServiceCollection AddConfigurations(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Set the JSON serializer options
            services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
            });

            // Set the API Behavior options
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(ValidateModelFilterAttribute));
                options.Filters.Add(new JsonPatchDocumentSnakeCaseFilter());
                options.ValueProviderFactories.Add(
                    new SnakeCaseQueryValueProvider.SnakeCaseQueryValueProviderFactory());
            });

            // Set the API Behavior options
            services.Configure<ApiBehaviorOptions>(options =>
            {
                //ConfigureApiBehaviorOptions- To disable automatic model validation
                //[For stop 400 response with default message] and
                ////validate filed with fluent validation and show our custom message
                options.SuppressModelStateInvalidFilter = true;
                //SuppressMapClientErrors suppress 404 client error body,default false. 
                options.SuppressMapClientErrors = true;
            });

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = configuration.GetValue<string>("IDENTITY_PROVIDER_SERVICE_HOST");
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = Audience.Universal
                    };
                });
            return services;
        }
        
        
        private static void RegisterInternalServices(this IServiceCollection services)
        {

        }

        private static void RegisterCorrelationIdServices(this IServiceCollection services)
        {
            services.AddCorrelationId(options =>
            {
                options.AddToLoggingScope = true;
                options.RequestHeader = CorrelationIdConfig.HeaderName;
                options.UpdateTraceIdentifier = true;
            }).WithGuidProvider();
        }
        private static void RegisterCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetValue<string>("Secrets:Redis:ConnectionString");
            services.AddStackExchangeRedisCache(options => { options.Configuration = redisConnectionString; });
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString!));
            services.AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));
        }

        private static void RegisterHealthCheck(this IServiceCollection services)
        {
            services.AddHealthChecks();
        }
    }
}