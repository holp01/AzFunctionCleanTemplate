﻿using AutoMapper;
using AzFunctionCleanTemplate.Application.Accounts.Dtos;
using AzFunctionCleanTemplate.Application.Interfaces;
using AzFunctionCleanTemplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionCleanTemplate.Application.Accounts
{
    public class AccountAppService : IAccountAppService
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IMapper _mapper;

        public AccountAppService(IRepository<Account> accountRepository,
            IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<AccountDto?> GetAccountById(Guid id, CancellationToken token)
        {
            var account = await _accountRepository.GetByIdAsync(id, token);
            if (account == null)
                return null;

            return _mapper.Map<AccountDto>(account);
        }
    }
}
