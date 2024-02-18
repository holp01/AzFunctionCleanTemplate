using AutoMapper;
using AzFunctionCleanTemplate.Application.Accounts;
using AzFunctionCleanTemplate.Application.Accounts.Dtos;
using AzFunctionCleanTemplate.Application.Interfaces;
using AzFunctionCleanTemplate.Domain.Entities;

namespace AzFunctionCleanTemplate.Infrastructure.Accounts
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
