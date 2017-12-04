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
            
            IEnumerable<JsonTestModel> posts = new WebConsumer().GetPosts();
        }
    }
}
