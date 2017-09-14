using System;
using System.Runtime.Serialization;

namespace GS.InsideGulfstream.Common.ExceptionHandling
{
    [Serializable]
    public class InsideGulfstreamException : Exception
    {
        public InsideGulfstreamException()
            : base()
        { }

        public InsideGulfstreamException(string message)
            : base(message)
        { }

        public InsideGulfstreamException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        public InsideGulfstreamException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public InsideGulfstreamException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        { }

        protected InsideGulfstreamException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
