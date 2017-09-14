using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace GS.ErrorLookupService.ServiceModel.Models
{
    [DataContract]
    public class QueryResult
    {
        private Collection<string[]> collection;
        private Collection<ErrorOutput> output;
        private Exception exception;

        public QueryResult(Collection<string[]> _collection)
        {
            this.collection = _collection;

            if (collection.Count > 0)
            {
                this.output = new Collection<ErrorOutput>();
                int count = 0;
                foreach (string[] obj in this.collection)
                {
                    if (count != 0)
                    {
                        this.output.Add(new ErrorOutput(obj[0], obj[1], obj[3], obj[4], obj[5], obj[6], obj[7]));
                    }
                    count++;
                }
            }
        }

        public QueryResult(Exception _exception)
        {
            this.exception = _exception;
        }

        public Collection<ErrorOutput> Output
        {
            get
            {
                return this.output;
            }
        }

        public string Message
        {
            get
            {
                if (this.exception != null)
                {
                    return this.exception.Message;
                }
                else
                    return "There is nothing to report.";
            }
        }

        public bool Exception
        {
            get
            {
                if (this.exception != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}