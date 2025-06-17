using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismApp.Modules.WMXLoader
{
    class WMXService : IWMXService
    {
        private IWMXApiClient _client = null;

        public WMXApiClient GetClient()
        {
            if (_client == null)
            {
                return null;
            }
            return (WMXApiClient)_client;
        }
    }
}
