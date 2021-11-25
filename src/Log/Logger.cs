using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Log
{
    public static class Logger
    {
        private static readonly JsonSerializerSettings JsonSettingsForLog = new()
        {
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffzzz",
            NullValueHandling = NullValueHandling.Ignore
        };

        private static string GetLogJsonString(LogModel model)
        {
            string json;
            try
            {
                model.CreationDate = DateTime.Now;
                json = JsonConvert.SerializeObject(model, JsonSettingsForLog);
            }
            catch
            {
                json = "";
            }

            return json;
        }

        private static void LogRedis(string log)
        {
            if (string.IsNullOrEmpty(log))
                return;
            Task.Run(() =>
            {
                try
                {
                    RedisHelper.Db.Publish("log_ui", log);
                }
                catch
                {
                    throw new ArgumentNullException("Couldnt Connect to redis server");
                }
            });
        }

        public static void Redis(LogModel model)
        {
            if (model == null)
                return;

            var log = GetLogJsonString(model);
            LogRedis(log);
        }

        public static void Log(LogModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (string.IsNullOrEmpty(model.Layer))
            {
                model.Layer = "ForexExchange.Worker";
            }

            model.MachineName = Environment.MachineName;

            Redis(model);
        }
    }
}
