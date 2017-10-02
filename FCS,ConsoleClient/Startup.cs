using FCS.Contracts;
using FCS.Core;

namespace FCS_ConsoleClient
{
    public class Startup
    {
        public static void Main()
        {
            IFileCacheConfiguration fileCacheConfiguration = new FileCacheConfiguration();
            ICacheService cacheService = new FileCache(fileCacheConfiguration);
        }
    }
}
