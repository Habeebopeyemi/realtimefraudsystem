using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using realtimefraudsystem.Models;

namespace realtimefraudsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly DBContext _dbContext;
        public TransactionsController(DBContext context)
        {
            _dbContext = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransaction()
        {
            return await _dbContext.AllTransactions.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(long id)
        {
            var transaction = await _dbContext.AllTransactions.FindAsync(id);

            if(transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            var newEntry = new Transaction
            {
                Id = transaction.Id,
                Country = transaction.Country,
                Timestamp = transaction.Timestamp,
                Amount = transaction.Amount,
                Balance = transaction.Balance,
                AccountNumber = transaction.AccountNumber,
                Status = transaction.Status,
                DestinationAccountNumber = transaction.DestinationAccountNumber,
                Transaction_id = transaction.Transaction_id,
            };

            _dbContext.AllTransactions.Add(newEntry);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction),new {id = newEntry.Id}, transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(long id, Transaction transaction)
        {
            if(id != transaction.Id)
            {
                return BadRequest();
            }

            var currentTransaction = await _dbContext.AllTransactions.FindAsync(id);

            if(currentTransaction == null)
            {
                return NotFound();
            }

            currentTransaction.Id = transaction.Id;
            currentTransaction.Country = transaction.Country;
            currentTransaction.Timestamp = transaction.Timestamp;
            currentTransaction.Amount = transaction.Amount;
            currentTransaction.Balance = transaction.Balance;
            currentTransaction.AccountNumber = transaction.AccountNumber;
            currentTransaction.Status = transaction.Status;
            currentTransaction.DestinationAccountNumber = transaction.DestinationAccountNumber;
            currentTransaction.Transaction_id = transaction.Transaction_id;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(long id) {
            var currentTransaction = await _dbContext.AllTransactions.FindAsync(id);

            if(currentTransaction == null)
            {
                return NotFound();
            }

            _dbContext.AllTransactions.Remove(currentTransaction);

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        private bool TransactionExists(long id)
        {
            return _dbContext.AllTransactions.Any(Transaction => Transaction.Id == id);
        }
    }
}
