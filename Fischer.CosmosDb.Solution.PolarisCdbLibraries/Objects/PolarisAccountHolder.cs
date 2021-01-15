using System;
using System.Collections.Generic;
using System.Text;

namespace Fischer.CosmosDb.Solution.PolarisCdbLibraries.Objects
{
    public class PolarisAccountHolder
    {
        public Guid id { get; set; }
        public Guid AccountGuid { get; set; }
        public string AccountHolder { get; set; }
        public string AccountType { get; set; }
    }
}
