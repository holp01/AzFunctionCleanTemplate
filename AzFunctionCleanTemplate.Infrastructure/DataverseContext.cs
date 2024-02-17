using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionCleanTemplate.Infrastructure
{
    public class DataverseContext
    {
        // This manages your connection and provides a service client for the Dataverse API.
        public ServiceClient ServiceClient { get; }

        public DataverseContext(string connectionString)
        {
            ServiceClient = new ServiceClient(connectionString);
        }
    }
}
