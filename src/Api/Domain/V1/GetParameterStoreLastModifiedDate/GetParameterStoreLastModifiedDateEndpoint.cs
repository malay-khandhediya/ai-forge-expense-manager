using System.Diagnostics.CodeAnalysis;
using NewRelic.Api.Agent;
using Peddle.ParameterStore.Shared.CacheIntegration;

namespace Peddle.ParameterStore.Domain.V1.GetParameterStoreLastModifiedDate;

public class GetParameterStoreLastModifiedDateEndpoint(
        ILogger<GetParameterStoreLastModifiedDateEndpoint> logger,
        ICacheService<string> parameterstoreCacheService,IParameterStoreService parameterstoreService,IConfiguration configuration)
    : FastEndpoints.Endpoint<GetParameterStoreLastModifiedDateRequestDto, GetParameterStoreLastModifiedDateResponseDto>
{
    [ExcludeFromCodeCoverage]
    public override void Configure()
    {
        Get("/retrieve-parameters-store-modification-details/{productName}");
        Version(1);
        DontCatchExceptions();
        Options(x => x.WithMetadata(new NewRelic.Api.Agent.TransactionAttribute())
            .Produces<GetParameterStoreLastModifiedDateResponseDto>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError)
        );
    }

    [Transaction]
    public override async Task HandleAsync(GetParameterStoreLastModifiedDateRequestDto request, CancellationToken ct)
    {
        logger.LogInformation("Get Parameter Store Last Modified Api request for {ProductName} started", request.ProductName);
        NewRelic.Api.Agent.NewRelic.SetTransactionName("Other", "Get Parameter Store Last Modified");
        string lastModifiedDate = await parameterstoreCacheService.GetOrCreateAsync(request.ProductName,
            async () => await parameterstoreService.GetLastModifiedDateForProductAsync(configuration,request.ProductName));
        var modifiedResponseDto =
            new GetParameterStoreLastModifiedDateResponseDto() { LastModifiedAt = lastModifiedDate };
        await SendOkAsync(modifiedResponseDto, ct);
    }
    
}