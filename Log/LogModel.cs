using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public class LogModel
    {
        public LogModel()
        {
            ApplicationName = "Forex Exchange Monitoring";
            Application = "ForexExchange.WebApp";
        }

        public string ApplicationName { get; private set; }
        public string Application { get; private set; }
        public string Layer { get; set; }
        public string MachineName { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.LogType EventType { get; set; }
        public string Priority { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string MessageDetail { get; set; }
        public object Exception { get; set; }
        public object User { get; set; }
        public object Client { get; set; }
        public object Extra { get; set; }

        /// <summary>
        /// yyyy-MM-ddTHH:mm:ss.fffffffzzz
        /// </summary>
        public DateTime CreationDate { get; set; }

        public object RequestParameters { get; set; }
    }
}
