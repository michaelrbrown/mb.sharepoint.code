using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GS.InsideGulfstream.Common.Json
{
    /// <summary>
    /// JSON Custom Date converter class
    /// </summary>
    public class JsonCustomDateConverter : DateTimeConverterBase
    {
        private TimeZoneInfo _timeZoneInfo;
        private string _dateFormat;

        /// <summary>
        /// Constructor for JSON Date Converter
        /// </summary>
        /// <param name="dateFormat">Date format</param>
        /// <param name="timeZoneInfo">Timezone info</param>
        public JsonCustomDateConverter(string dateFormat, TimeZoneInfo timeZoneInfo)
        {
            _dateFormat = dateFormat;
            _timeZoneInfo = timeZoneInfo;
        }

        /// <summary>
        /// Can this object be converted
        /// </summary>
        /// <param name="objectType">Object to convert</param>
        /// <returns>True if the object can be converted, false if not</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to write JSON to a custom formatted DateTime format
        /// </summary>
        /// <param name="writer">JsonWriter object</param>
        /// <param name="value">object value</param>
        /// <param name="serializer">Json Serializer</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(value), _timeZoneInfo).ToString(_dateFormat));
            writer.Flush();
        }
    }
}
