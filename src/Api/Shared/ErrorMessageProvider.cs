using System.Net;

namespace Peddle.ParameterStore.Shared;

public static class ErrorResponsesProvider
{
    public static readonly Foundation.Common.Middleware.Errors.ErrorResponsesProvider InvalidRequestParameters = new(
        "invalid_request_parameters", "One or more parameters invalid", "invalid_request_parameters",
        HttpStatusCode.BadRequest);
}