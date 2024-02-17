using AutoMapper;
using AzFunctionCleanTemplate.Application.Accounts.Dtos;
using AzFunctionCleanTemplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionCleanTemplate.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountDto>();
            // CreateMap<AccountDto, Account>(); // If you need reverse mapping
            // Add other mappings here
        }
    }
}
