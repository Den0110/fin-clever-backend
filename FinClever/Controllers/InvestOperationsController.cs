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
        private readonly IStockRepository stockRepository;
        private readonly ICurrencyRepository currencyRepository;

        public InvestOperationsController(
            IInvestOperationRepository operationRepository,
            IStockRepository stockRepository,
            ICurrencyRepository currencyRepository
        )
        {
            this.operationRepository = operationRepository;
            this.stockRepository = stockRepository;
            this.currencyRepository = currencyRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<InvestOperation>> GetOperations(string? ticker)
        {
            return ticker == null ? await operationRepository.Get(User.GetId()) :
                await operationRepository.GetForTicker(User.GetId(), ticker);
        }

        [HttpPost]
        public async Task<ActionResult<InvestOperation>> PostOperation([FromBody] InvestOperation operation)
        {
            operation.UserId = User.GetId();
            var price = await stockRepository.GetStock(operation.Ticker);
            if (price?.CurrentPrice == 0)
            {
                return BadRequest();
            }
            if (operation.UsdPrice == 0.0)
            {
                operation.UsdPrice = await currencyRepository.GetUsdRate();
            }
            var newOperation = await operationRepository.Create(operation);
            return CreatedAtAction(nameof(GetOperations), new { Id = newOperation.Id }, newOperation);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOperation(int id)
        {
            var operation = await operationRepository.Get(User.GetId(), id);
            if (operation == null)
                return NotFound();
            await operationRepository.Delete(User.GetId(), id);
            return NoContent();
        }
    }
}
