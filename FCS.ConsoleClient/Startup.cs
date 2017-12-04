using System;
using System.Collections.Generic;

using FCS.Contracts;
using FCS.Core;
using FCS.Models;
using FCS.Utils;

namespace FCS_ConsoleClient
{
    public class Startup
    {
        public static void Main()
        {
            ICacheService cacheService = new FileCache();
            WebConsumer webConsumer = new WebConsumer();

            IEnumerable<JsonTestModel> posts = webConsumer.GetPosts();

            string str = cacheService.Get("Test", () => webConsumer.GetJsonPosts(), 1000);

            Console.WriteLine(str);
        }
    }
}
