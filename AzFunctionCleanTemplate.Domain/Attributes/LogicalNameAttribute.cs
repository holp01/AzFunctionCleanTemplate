using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionCleanTemplate.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LogicalNameAttribute : Attribute
    {
        public string Name { get; }

        public LogicalNameAttribute(string name)
        {
            Name = name;
        }
    }
}
