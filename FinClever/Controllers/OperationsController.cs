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

        private readonly IOperationRepository repository;
        private readonly IAccountRepository accountrepository;

        public OperationsController(IOperationRepository repository, IAccountRepository accountrepository)
        {
            this.repository = repository;
            this.accountrepository = accountrepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Operation>> GetOperations()
        {
            return await repository.Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Operation>> GetOperation(int id)
        {
            return await repository.Get(id);
        }

        [HttpPost]
        public async Task<ActionResult<Operation>> PostOperation([FromBody] Operation operation)
        {
            var newOperation = await repository.Create(operation);
            return CreatedAtAction(nameof(GetOperations), new { Id = newOperation.Id }, newOperation);
        }

        [HttpPut]
        public async Task<ActionResult> PutOperation(int id, [FromBody] Operation operation)
        {
            if (id != operation.Id) 
                return BadRequest();
            await repository.Update(operation);
            return NoContent();
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
