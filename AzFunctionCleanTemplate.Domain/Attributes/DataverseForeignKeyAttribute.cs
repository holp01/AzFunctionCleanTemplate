using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionCleanTemplate.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataverseForeignKeyAttribute : Attribute
    {
        public string ForeignKey { get; }

        public DataverseForeignKeyAttribute(string foreignKey)
        {
            ForeignKey = foreignKey;
        }
    }
}
