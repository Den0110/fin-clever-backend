using FinClever.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinClever.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/accounts")]
    public class AccountsController : ControllerBase
    {

        private readonly IAccountRepository repository;

        public AccountsController(IAccountRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await repository.Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            return await repository.Get(id);
        }

        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount([FromBody] Account account)
        {
            account.UserId = User.GetId();
            var newAccount = await repository.Create(account);
            return CreatedAtAction(nameof(GetAccounts), new { Id = newAccount.Id }, newAccount);
        }

        [HttpPut]
        public async Task<ActionResult> PutAccount(int id, [FromBody] Account account)
        {
            if (id != account.Id) 
                return BadRequest();
            await repository.Update(account);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccount(int id)
        {
            var account = await repository.Get(id);
            if (account == null) 
                return NotFound();
            await repository.Delete(id);
            return NoContent();
        }
    }
}
