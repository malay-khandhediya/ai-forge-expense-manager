using System.Diagnostics.CodeAnalysis;

namespace Peddle.ParameterStore.Domain.V1.GetParameterStoreLastModifiedDate
{
    [ExcludeFromCodeCoverage]
    public class GetParameterStoreLastModifiedDateRequestDto
    {
        public required string ProductName { get; set; }
    }
}
