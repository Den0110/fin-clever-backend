using FinClever.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinClever.Controllers
{
    [ApiController]
    [Route("/api/operations")]
    public class OperationsController : ControllerBase
    {

        private readonly IOperationRepository operationRepository;
        private readonly IAccountRepository accountRepository;

        public OperationsController(IOperationRepository operationRepository, IAccountRepository accountRepository)
        {
            this.operationRepository = operationRepository;
            this.accountRepository = accountRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Operation>> GetOperations()
        {
            return await operationRepository.Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Operation>> GetOperation(int id)
        {
            return await operationRepository.Get(id);
        }

        [HttpPost]
        public async Task<ActionResult<Operation>> PostOperation([FromBody] Operation operation)
        {
            var newOperation = await operationRepository.Create(operation);

            var account = await accountRepository.Get(operation.AccountId);
            if (account == null)
                return NotFound();
            account.Balance += operation.AbsoluteBalanceEffect();
            await accountRepository.Update(account);

            return CreatedAtAction(nameof(GetOperations), new { Id = newOperation.Id }, newOperation);
        }

        [HttpPut]
        public async Task<ActionResult> PutOperation(int id, [FromBody] Operation operation)
        {
            if (id != operation.Id)
                return BadRequest();

            var oldOperation = await operationRepository.Get(id);
            if (oldOperation.AccountId == operation.AccountId)
            {
                var account = await accountRepository.Get(operation.AccountId);
                if (account == null)
                    return NotFound();
                account.Balance += operation.AbsoluteBalanceEffect() - oldOperation.AbsoluteBalanceEffect();
                await accountRepository.Update(account);
            } else
            {
                var oldAccount = await accountRepository.Get(oldOperation.AccountId);
                var account = await accountRepository.Get(operation.AccountId);
                if (oldAccount == null || account == null)
                    return NotFound();
                oldAccount.Balance -= oldOperation.AbsoluteBalanceEffect();
                account.Balance += operation.AbsoluteBalanceEffect();
                await accountRepository.Update(oldAccount);
                await accountRepository.Update(account);
            }

            await operationRepository.Update(operation); // TODO: fix error here
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOperation(int id)
        {
            var operation = await operationRepository.Get(id);
            if (operation == null)
                return NotFound();

            var account = await accountRepository.Get(operation.AccountId);
            if (account == null)
                return NotFound();
            account.Balance -= operation.AbsoluteBalanceEffect();
            await accountRepository.Update(account);

            await operationRepository.Delete(id);
            return NoContent();
        }
    }
}
