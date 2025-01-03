using System.Collections;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Peddle.ParameterStore.Shared.CacheIntegration;

    public class CacheService<T> : ICacheService<T>
    {
        private readonly ILogger<CacheService<T>> _log;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public  int DefaultCacheExpiryInSeconds { get; set; }

        private string CacheRegion => string.IsNullOrEmpty(Region)
            ? "{SSMLastModified_By_Product}"
            : "{" + Region + "}";

        public string Region { get; set; }

        public CacheService(ILogger<CacheService<T>> log, IDistributedCache distributedCache,
            IConnectionMultiplexer connectionMultiplexer)
        {
            _log = log;
            _log.LogInformation("Initialize cache manager instance of {FullName}", typeof(T).FullName);
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        #region Distributed Cahche Methods

        
        public async Task<T?> GetOrCreateAsync(string key, Func<ValueTask<T>> getMethod)
        {
            var value = await GetItemAsync(key);
            if (value != null)
            {
                return value;
            }

            value = await getMethod();
            if (value == null) return value;
            if (value is IList { Count: 0 }) return value;
            
            await SetItemAsync(key, value, DefaultCacheExpiryInSeconds);
            return value;
        }
        
        public Task<T> GetItemAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            return GetItemAsync(key, CacheRegion);
        }

        public Task SetItemAsync(string key, T value, double? expireInSeconds = null)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            _log.LogInformation("Set Item to cache for key {@Region}:{@CacheKey}", CacheRegion, key);
            return SetItemAsync(key, value, CacheRegion, expireInSeconds);
        }
        

        private async Task<T> GetItemAsync(string key, string region)
        {
            var itemKey = string.IsNullOrWhiteSpace(region) ? key : $"{region}:{key}";
            var value = default(T);
            try
            {
                var item = await _distributedCache.GetStringAsync(itemKey);
                value = item != null ? JsonSerializer.Deserialize<T>(item) : default;
                if (value != null)
                {
                    _log.LogInformation("Item found for key {@Region}:{@CacheKey} from cache", CacheRegion, key);
                }
            }
            catch (Exception exception)
            {
                _log.LogWarning(exception, null, "Redis failed to Get the key with key name: " + itemKey);
            }

            return value;
        }

        private async Task SetItemAsync(string key, T value, string region, double? expireInSeconds = null)
        {
            var itemKey = string.IsNullOrWhiteSpace(region) ? key : $"{region}:{key}";
            try
            {
                var strValue = value != null ? JsonSerializer.Serialize(value) : default;
                var distributedCacheEntryOptions = GetDistributedCacheEntryOptions(expireInSeconds);
                if (!string.IsNullOrEmpty(strValue))
                {
                    if (distributedCacheEntryOptions != null)
                        await _distributedCache.SetStringAsync(itemKey, strValue, distributedCacheEntryOptions);
                    else
                        await _distributedCache.SetStringAsync(itemKey, strValue);
                }
            }
            catch (Exception exception)
            {
                _log.LogWarning(exception, null, "Redis failed to upsert the key with key name: " + itemKey);
            }
            
        }

        private static DistributedCacheEntryOptions GetDistributedCacheEntryOptions(double? expireInSeconds)
        {
            DistributedCacheEntryOptions distributedCacheEntryOptions = null;
            if (expireInSeconds is > 0)
            {
                distributedCacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds((double)expireInSeconds),
                };
            }

            return distributedCacheEntryOptions;
        }

        #endregion
    }

public interface ICacheService<T>
{
    public string Region { get; set; }
    Task<T> GetItemAsync(string key);
    Task SetItemAsync(string key, T value, double? expireInSeconds = null);
    Task<T?> GetOrCreateAsync(string key, Func<ValueTask<T>> getMethod);

}
