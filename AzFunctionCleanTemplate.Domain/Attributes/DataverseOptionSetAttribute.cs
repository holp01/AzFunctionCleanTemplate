﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionCleanTemplate.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataverseOptionSetAttribute : Attribute
    {
        public string OptionSetName { get; }

        public DataverseOptionSetAttribute(string optionSetName)
        {
            OptionSetName = optionSetName;
        }
    }
}
