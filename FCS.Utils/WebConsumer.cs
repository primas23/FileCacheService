using System.Collections.Generic;
using System.Net;

using Newtonsoft.Json;

using FCS.Models;

namespace FCS.Utils
{
    public class WebConsumer
    {
        private readonly string jsonUrlPosts = "https://jsonplaceholder.typicode.com/posts";
        
        public IEnumerable<JsonTestModel> GetPosts()
        {
            IEnumerable<JsonTestModel> posts = new List<JsonTestModel>();
            string json = string.Empty;

            using (WebClient webClien = new WebClient())
            {
                json = webClien.DownloadString(jsonUrlPosts);
            }

            if (!string.IsNullOrWhiteSpace(json))
            {
                posts = JsonConvert.DeserializeObject<IEnumerable<JsonTestModel>>(json);
            }

            return posts;
        }
    }
}
