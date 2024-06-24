using Microsoft.AspNetCore.Mvc;
using Store_Sys.DTOs;
using Store_Sys.Modules;
using WebApplication1.UnitOfWork;

namespace Store_Sys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
        {
            var transactions = await _unitOfWork.Transactions.GetAllAsync();
            var transactionDtos = transactions.Select(transaction => new TransactionDto
            {
                Id = transaction.Id,
                CustomerId = transaction.CustomerId,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Type = transaction.Type
            });
            return Ok(transactionDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            var transactionDto = new TransactionDto
            {
                Id = transaction.Id,
                CustomerId = transaction.CustomerId,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Type = transaction.Type
            };
            return Ok(transactionDto);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> PostTransaction(TransactionDto transactionDto)
        {
            var transaction = new Transaction
            {
                CustomerId = transactionDto.CustomerId,
                Amount = transactionDto.Amount,
                Date = transactionDto.Date,
                Type = transactionDto.Type
            };
            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.CompleteAsync();

            transactionDto.Id = transaction.Id;
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transactionDto);
        }
    }
}
