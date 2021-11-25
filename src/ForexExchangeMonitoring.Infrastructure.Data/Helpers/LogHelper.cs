using Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Infrastructure.Data.Helpers
{
    public static class LogHelper
    {
        public static void Log(LogModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (string.IsNullOrWhiteSpace(model.Layer))
            {
                model.Layer = "ForexExchange.InfraStructure.Data";
            }

            model.MachineName = Environment.MachineName;

            Logger.Redis(model);
        }
    }
}
