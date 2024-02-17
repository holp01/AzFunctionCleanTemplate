using AzFunctionCleanTemplate.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionCleanTemplate.Domain.Entities
{
    [DataverseTableName("account")]
    public class Account
    {
        [LogicalName("accountid")]
        public Guid Id { get; set; }

        [LogicalName("name")]
        public string Name { get; set; }
    }
}
