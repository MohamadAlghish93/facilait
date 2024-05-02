using Microsoft.Extensions.Caching.Memory;

namespace FacilaIT.Helper.Shared;


public static class CacheManager
{
    public static IMemoryCache Cache { get; set; }
}