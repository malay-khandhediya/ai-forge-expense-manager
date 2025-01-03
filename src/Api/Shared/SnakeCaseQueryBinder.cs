using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using FastEndpoints;
using Peddle.Foundation.Common.Dtos;

namespace Peddle.ParameterStore.Shared
{
    public class SnakeCaseQueryBinder
    {
        public static void SnakeCaseBindFunc(object request, Type tRequest, BinderContext context, CancellationToken cancellationToken)
        {
            object Parse(string valueToConvert, Type dataType)
            {
                try
                {
                    var obj = TypeDescriptor.GetConverter(dataType);
                    var value = obj.ConvertFromString(null, CultureInfo.InvariantCulture, valueToConvert);
                    return value;
                }
                catch (NotSupportedException)
                {
                    throw new BusinessException(Foundation.Common.Middleware.Errors.ErrorResponsesProvider.InvalidRequestParameters.InternalCode);
                }
                catch (FormatException)
                {
                    throw new BusinessException(Foundation.Common.Middleware.Errors.ErrorResponsesProvider.InvalidRequestParameters.InternalCode);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            foreach (var param in context.HttpContext.Request.Query)
            {
                var property = request.GetType()
                    .GetProperty(param.Key.Replace("_", ""), BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                property?.SetValue(request, Parse(param.Value[0]!, property.PropertyType));
            }
        }
    }
}
