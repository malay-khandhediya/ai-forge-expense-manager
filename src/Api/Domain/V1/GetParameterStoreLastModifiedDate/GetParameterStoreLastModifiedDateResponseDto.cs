using System.Diagnostics.CodeAnalysis;

namespace Peddle.ParameterStore.Domain.V1.GetParameterStoreLastModifiedDate
{
    [ExcludeFromCodeCoverage]
    public class GetParameterStoreLastModifiedDateResponseDto
    {
        public required string LastModifiedAt { get; set; }
    }
}
