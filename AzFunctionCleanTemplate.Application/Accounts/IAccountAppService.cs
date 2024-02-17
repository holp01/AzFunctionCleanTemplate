using AzFunctionCleanTemplate.Application.Accounts.Dtos;
using AzFunctionCleanTemplate.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionCleanTemplate.Application.Accounts
{
    public interface IAccountAppService : IApplicationBase
    {
        Task<AccountDto?> GetAccountById(Guid id, CancellationToken token);
    }
}
