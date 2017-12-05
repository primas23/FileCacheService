using Newtonsoft.Json;

using FCS.Contracts;

namespace FCS.Utils
{
    public class JosnService : IJosnService
    {
        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>
        /// The deserialized object
        /// </returns>
        public T DeserializeObject<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Returns serialized object.</returns>
        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
