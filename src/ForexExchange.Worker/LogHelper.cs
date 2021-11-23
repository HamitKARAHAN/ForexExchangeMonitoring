using Log;
using System;

namespace ForexExchange.Worker
{
    public static class LogHelper
    {
        public static void Log(LogModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (string.IsNullOrWhiteSpace(model.Layer))
            {
                model.Layer = "Digiturk.Dzdy.Business";
            }

            model.MachineName = Environment.MachineName;

            Logger.Redis(model);
        }
    }
}
