using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation.Runspaces;

namespace GS.ErrorLookupService.ServiceModel.Models
{
    public class CorrelationQuery
    {
        Collection<string[]> _results;
        private string _savePath, _mergeCommand;

        public DateTime StartDateTime
        {
            get;
            set;
        }

        public DateTime EndDateTime
        {
            get;
            set;
        }

        public String CorrelationId
        {
            get;
            set;
        }

        public String SavePath
        {
            get { return _savePath; }
        }

        public String MergeCommand
        {
            get { return _mergeCommand; }
        }

        public QueryResult Result
        {
            get { return new QueryResult(_results); }
        }

        public CorrelationQuery(DateTime startDateTime, DateTime endDateTime, string correlationId)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            CorrelationId = correlationId;
            _results = new Collection<string[]>();
        }

        /// <summary>
        /// Method to get SharePoint error info from combining ULS logs across all servers (works for load balanced server farm)
        /// </summary>
        /// <param name="startDateTime">Start DateTime for logs</param>
        /// <param name="endDateTime">End DateTime for logs</param>
        /// <param name="correlationId">Correlation Id to search for</param>
        public void QueryServers(DateTime startDateTime, DateTime endDateTime, string correlationId)
        {
            // Local path to save log information
            _savePath = String.Format("{0}{1}.log", Path.GetTempPath(), System.Guid.NewGuid().ToString());
            // SharePoint PS Merge Command (merge all log files from all web front ends)
            _mergeCommand = String.Format("Merge-SPLogFile -OverWrite -Path '{0}' -StartTime '{1}' -EndTime '{2}' -Correlation '{3}'", _savePath, startDateTime, endDateTime, correlationId);
            // Run powershell script
            InitialSessionState iss = InitialSessionState.CreateDefault();
            PSSnapInException warning;
            iss.ImportPSSnapIn("Microsoft.SharePoint.PowerShell", out warning);
            Runspace runspace = RunspaceFactory.CreateRunspace(iss);
            runspace.Open();
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(_mergeCommand);
            try
            {
                pipeline.Invoke();
            }
            catch (Exception ex)
            {
                File.Delete(_savePath);
                throw ex;
            }
            // Get log file from powershell script
            if (File.Exists(_savePath))
            {
                var sReader = new StreamReader(_savePath);
                string inputLine = "";

                string[] values = null;
                _results = new Collection<string[]>();
                // Read all error information into results
                while ((inputLine = sReader.ReadLine()) != null)
                {
                    values = inputLine.Split('\t');
                    _results.Add(values);
                }
                sReader.Close();
            }
            else
            {
                throw new FileNotFoundException("No results were found.");
            }
            // Cleanup
            runspace.Close();
            File.Delete(_savePath);
        }

    }
}