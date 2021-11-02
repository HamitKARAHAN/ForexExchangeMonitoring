using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Application.Interfaces
{
    public interface IWorkerService
    {
        public void TakeDataFromApi();
    }
}
