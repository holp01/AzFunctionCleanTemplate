using AzFunctionCleanTemplate.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionCleanTemplate.Application.Accounts.Dtos
{
    public class AccountDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
