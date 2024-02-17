using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionCleanTemplate.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataverseTableNameAttribute : Attribute
    {
        public string TableName { get; }

        public DataverseTableNameAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
