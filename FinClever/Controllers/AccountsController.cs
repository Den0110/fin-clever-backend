using FinClever.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinClever.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/accounts")]
    public class AccountsController : ControllerBase
    {

        private readonly IAccountRepository accountRepository;
        private readonly IInvestOperationRepository investOperationRepository;
        private readonly IPortfolioRepository portfolioRepository;
        private readonly IStockRepository stockRepository;
        private readonly ICurrencyRepository currencyRepository;

        public AccountsController(
            IAccountRepository accountRepository,
            IInvestOperationRepository investOperationRepository,
            IPortfolioRepository portfolioRepository,
            IStockRepository stockRepository,
            ICurrencyRepository currencyRepository
        )
        {
            this.accountRepository = accountRepository;
            this.investOperationRepository = investOperationRepository;
            this.portfolioRepository = portfolioRepository;
            this.stockRepository = stockRepository;
            this.currencyRepository = currencyRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Account>> GetAccounts()
        {
            var accounts = await accountRepository.Get();
            if(await investOperationRepository.HasOne(User.GetId()))
            {
                var brokerAcc = new Account();
                brokerAcc.Name = "Брокерский счет";
                brokerAcc.Type = "brokerage-account";
                brokerAcc.Balance = await GetBrokerAccountBalance();
                accounts.Add(brokerAcc);
            }
            return accounts;
        }

        private async Task<float> GetBrokerAccountBalance()
        {
            var totalPrice = .0;
            var stocks = await portfolioRepository.GetStocks(User.GetId());

            foreach (var s in stocks)
            {
                var ticker = s.Ticker;
                var stockInfo = await stockRepository.GetStock(ticker);
                if (stockInfo != null)
                {
                    s.CurrentPrice = stockInfo.CurrentPrice;
                    s.CompanyName = ticker;
                    totalPrice += s.CurrentPrice * s.Amount ?? .0;
                }
            }
            var usrRate = await currencyRepository.GetUsdRate();
            return (float) (usrRate * totalPrice);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            return await accountRepository.Get(id);
        }

        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount([FromBody] Account account)
        {
            account.UserId = User.GetId();
            var newAccount = await accountRepository.Create(account);
            return CreatedAtAction(nameof(GetAccounts), new { Id = newAccount.Id }, newAccount);
        }

        [HttpPut]
        public async Task<ActionResult> PutAccount(int id, [FromBody] Account account)
        {
            if (id != account.Id) 
                return BadRequest();
            await accountRepository.Update(account);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccount(int id)
        {
            var account = await accountRepository.Get(id);
            if (account == null) 
                return NotFound();
            await accountRepository.Delete(id);
            return NoContent();
        }
    }
}
