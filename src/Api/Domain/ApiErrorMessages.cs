using System.Net;
using Peddle.Foundation.Common.Middleware.Errors;

namespace Peddle.ParameterStore.Domain
{
    public static class ApiErrorMessages
    {
        public static readonly ErrorResponsesProvider InvalidProductName = new("invalid_product_name", "Product name invalid", "invalid_product_name", HttpStatusCode.BadRequest);
    }
}
