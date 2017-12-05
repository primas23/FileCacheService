namespace FCS.Contracts
{
    public interface IJosnService
    {
        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The deserialized object.</returns>
        T DeserializeObject<T>(string content);

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Returns serialized object.</returns>
        string SerializeObject(object value);
    }
}
