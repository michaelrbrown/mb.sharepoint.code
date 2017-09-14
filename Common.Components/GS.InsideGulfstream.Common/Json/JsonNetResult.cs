using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GS.InsideGulfstream.Common.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace GS.InsideGulfstream.Common.Json
{
    public class JsonNetResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }
        public JsonSerializerSettings SerializerSettings { get; set; }

        private Formatting _formatting = Formatting.Indented;
        public Formatting JsonFormatting
        {
            get
            {
                return _formatting;
            }
            set
            {
                _formatting = value;
            }
        }

        private DateFormatHandling _dateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
        public DateFormatHandling JsonDateFormatHandling
        {
            get
            {
                return _dateFormatHandling;
            }
            set
            {
                _dateFormatHandling = value;
            }
        }

        private NullValueHandling _nullValueHandling = NullValueHandling.Ignore;
        public NullValueHandling JsonNullValueHandling
        {
            get
            {
                return _nullValueHandling;
            }
            set
            {
                _nullValueHandling = value;
            }
        }

        private DateTimeZoneHandling _dateTimeZoneHandling = DateTimeZoneHandling.Utc;
        public DateTimeZoneHandling JsonDateTimeZoneHandling
        {
            get
            {
                return _dateTimeZoneHandling;
            }
            set
            {
                _dateTimeZoneHandling = value;
            }
        }

        private string _dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public string JsonDateTimeFormat
        {
            get
            {
                return _dateTimeFormat;
            }
            set
            {
                _dateTimeFormat = value;
            }
        }

        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType)
              ? ContentType
              : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            
            if (Data != null)
            {
                // New instance of JsonTextWriter
                JsonTextWriter writer = new JsonTextWriter(response.Output) {
                    Formatting = JsonFormatting,
                    DateFormatHandling = JsonDateFormatHandling,
                    DateTimeZoneHandling = JsonDateTimeZoneHandling
                };
                // DateTime custom convertor
                //SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = JsonDateTimeFormat });
                SerializerSettings.Converters.Add(new JsonCustomDateConverter(JsonDateTimeFormat, TimeZoneInfo.Utc));
                // Ignore circular references
                SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // Null value handling
                SerializerSettings.NullValueHandling = JsonNullValueHandling;
                //SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                // Pass serializer settings
                JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
                // Serialize json with JsonTextWriter
                serializer.Serialize(writer, Data);
                // Flush object
                writer.Flush();
            }
        }
    }
}
