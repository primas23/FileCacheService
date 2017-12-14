using System;

using FCS.Core;
using FCS.Utils;
using FCS.Common;

namespace FCS_ConsoleClient
{
    public class Startup
    {
        public static void Main()
        {
            var cacheService = new FileCache(
                new FileRead(),
                new FileWrite(),
                new DateTimeProvider(),
                new JosnService(),
                new FileOperationsProvider(),
                new DirectoryOperationsProvider());

            var cacheManager = new CacheManager(cacheService, new DirectoryOperationsProvider()));
            cacheManager.ScopeName = "IndexPDTs";

            var webConsumer = new WebConsumer();

            var post1 = cacheManager.Get("id" + "_" + 1.ToString(), () => webConsumer.GetMockedDataPosts(1), GlobalConstants.OneWeekInSeconds);
            var post2 = cacheManager.Get("id" + "_" + 2.ToString(), () => webConsumer.GetMockedDataPosts(2), GlobalConstants.OneWeekInSeconds);


            cacheManager.Dispose();
            Console.WriteLine();
        }
    }
}
