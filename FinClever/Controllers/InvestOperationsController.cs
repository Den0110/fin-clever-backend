using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinClever.Models;
using FinClever.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinClever.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/invest/operations")]
    public class InvestOperationsController : ControllerBase
    {
        private readonly IInvestOperationRepository repository;

        public InvestOperationsController(IInvestOperationRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<InvestOperation>> GetOperations(string? ticker)
        {
            return ticker == null ? await repository.Get() : await repository.GetForTicker(ticker);
        }

        [HttpPost]
        public async Task<ActionResult<InvestOperation>> PostOperation([FromBody] InvestOperation operation)
        {
            operation.UserId = User.GetId();
            var newOperation = await repository.Create(operation);
            return CreatedAtAction(nameof(GetOperations), new { Id = newOperation.Id }, newOperation);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOperation(int id)
        {
            var operation = await repository.Get(id);
            if (operation == null)
                return NotFound();
            await repository.Delete(id);
            return NoContent();
        }
    }
}
