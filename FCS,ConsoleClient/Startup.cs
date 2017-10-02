using FCS.Contracts;
using FCS.Core;

namespace FCS_ConsoleClient
{
    public class Startup
    {
        public static void Main()
        {
            ICacheService cacheService = new FileCache();
        }
    }
}
