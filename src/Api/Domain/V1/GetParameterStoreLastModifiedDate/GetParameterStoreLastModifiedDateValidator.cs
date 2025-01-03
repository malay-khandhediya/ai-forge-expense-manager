using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Peddle.Foundation.Common.Extensions;
using Peddle.Foundation.Common.Middleware.Errors;

namespace Peddle.ParameterStore.Domain.V1.GetParameterStoreLastModifiedDate
{
    public class GetParameterStoreLastModifiedDateValidator : AbstractValidator<GetParameterStoreLastModifiedDateRequestDto>
    {
        [ExcludeFromCodeCoverage]
        public GetParameterStoreLastModifiedDateValidator(IServiceProvider serviceProvider)
        {
            
            RuleFor(m => m)
                .Must((query, cancellation) => !query.ProductName.IsEmpty())
                .WithErrorCode(ErrorResponsesProvider.InvalidRequestParameters.InternalCode);
            
            
            RuleFor(m => m.ProductName)
                .Must(x => IsValidProduct(x!))
                .WithErrorCode(ApiErrorMessages.InvalidProductName.InternalCode);
        }
        private static bool IsValidProduct(string? type)
        {
            return (type is not null) && (EnumExtensionV2.TryParseWithEnumMemberNameV2<Product>(type, out _));
        }
        

    }
}
