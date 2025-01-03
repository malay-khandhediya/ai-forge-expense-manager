using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
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
            services.RegisterCaching(configuration);
            services.RegisterInternalServices();
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


        
        
        private static void RegisterInternalServices(this IServiceCollection services)
        {

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