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
        private readonly IInvestOperationRepository operationRepository;
        private readonly ICurrencyRepository currencyRepository;

        public InvestOperationsController(IInvestOperationRepository operationRepository, ICurrencyRepository currencyRepository)
        {
            this.operationRepository = operationRepository;
            this.currencyRepository = currencyRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<InvestOperation>> GetOperations(string? ticker)
        {
            return ticker == null ? await operationRepository.Get() :
                await operationRepository.GetForTicker(ticker);
        }

        [HttpPost]
        public async Task<ActionResult<InvestOperation>> PostOperation([FromBody] InvestOperation operation)
        {
            operation.UserId = User.GetId();
            if(operation.UsdPrice == 0.0)
            {
                operation.UsdPrice = await currencyRepository.GetUsdRate();
            }
            var newOperation = await operationRepository.Create(operation);
            return CreatedAtAction(nameof(GetOperations), new { Id = newOperation.Id }, newOperation);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOperation(int id)
        {
            var operation = await operationRepository.Get(id);
            if (operation == null)
                return NotFound();
            await operationRepository.Delete(id);
            return NoContent();
        }
    }
}
