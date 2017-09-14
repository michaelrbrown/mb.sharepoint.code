using Newtonsoft.Json;
using System;

namespace GS.InsideGulfstream.Common.Json
{
    /// <summary>
    /// JSON custom append field convertor class for Uri types
    /// </summary>
    public class JsonUriAppendFieldConverter : JsonConverter
    {
        private string _fieldToAppend;

        /// <summary>
        /// Constructor for JSON Append Field Converter
        /// </summary>
        /// <param name="fieldToAppend">Field to append</param>
        public JsonUriAppendFieldConverter(string fieldToAppend)
        {
            _fieldToAppend = fieldToAppend;
        }

        /// <summary>
        /// Can this object be converted
        /// </summary>
        /// <param name="objectType">Object to convert</param>
        /// <returns>True if the object can be converted, false if not</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Uri));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return new Uri((string)reader.Value + _fieldToAppend);
            }

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            throw new InvalidOperationException("Unhandled case for UriConverter. Check to see if this converter has been applied to the wrong serialization type.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (null == value)
            {
                writer.WriteNull();
                return;
            }

            if (value is Uri)
            {
                writer.WriteValue(((Uri)value).OriginalString + _fieldToAppend);
                return;
            }

            throw new InvalidOperationException("Unhandled case for UriConverter. Check to see if this converter has been applied to the wrong serialization type.");
        }

    }
}
