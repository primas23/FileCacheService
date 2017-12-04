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

        public string GetJsonPosts()
        {            
            string json = string.Empty;

            using (WebClient webClien = new WebClient())
            {
                json = webClien.DownloadString(jsonUrlPosts);
            }
            
            return json;
        }

        public IEnumerable<JsonTestModel> GetMockedDataPosts()
        {
            IEnumerable<JsonTestModel> mockedData = new List<JsonTestModel>()
            {
                new JsonTestModel
                {
                    Id = 1,
                    UserId = 1,
                    Title = "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
                    Body = "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
                },
                new JsonTestModel
                {
                    Id = 2,
                    UserId = 2,
                    Title = "qui est esse",
                    Body = "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
                }
            };

            return mockedData;
        }
    }
}
