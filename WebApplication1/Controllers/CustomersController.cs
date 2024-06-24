using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_Sys.DTOs;
using Store_Sys.Modules;
using WebApplication1.Modules;
using WebApplication1.UnitOfWork;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomersController(IUnitOfWork unitOfWork,  IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            //var customers = await _unitOfWork.Customers.GetAllAsync();
            //var customerDtos = customers.Select(customer => new CustomerDto
            //{
            //    Id = customer.Id,
            //    Name = customer.Name,
            //    Email = customer.Email,
            //   // Balance = customer.Balance
            //});
            //return Ok(customerDtos);
            var customers = await _unitOfWork.Customers.GetAllAsync();
            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return Ok(customerDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            var customerDto = _mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.CompleteAsync();

            customerDto.Id = customer.Id;
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customerDto);
        }

        [HttpPost("deposit")]
        public async Task<ActionResult> Deposit([FromBody] TransactionDto transactionDto)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(transactionDto.CustomerId);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            var transaction = new Transaction
            {
                CustomerId = transactionDto.CustomerId,
                Amount = transactionDto.Amount,
                Date = DateTime.Now,
                Type = "Deposit"
            };

            //customer.Balance += transaction.Amount;
            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpGet("{id}/statement")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetCustomerStatement(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            var transactions = await _unitOfWork.Transactions.FindAsync(t => t.CustomerId == id);
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
        //[HttpGet("{id}/balance")]
        //public async Task<ActionResult<decimal>> GetCustomerBalance(int id)
        //{
        //    var balance = await _unitOfWork.GetCustomerBalanceAsync(id);
        //    if (balance == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(balance.Balance);
        //}

        [HttpGet("{id}/balance")]
        public async Task<ActionResult<decimal>> GetCustomerBalance(int id)
        {
            var balance = await _unitOfWork.Customers.GetCustomerBalanceAsync(id);
            if (balance == null)
            {
                return NotFound();
            }
            return Ok(balance.Balance);
        }
        private async Task<bool> CustomerExists(int id)
        {
            return (await _unitOfWork.Customers.GetByIdAsync(id)) != null;
        }

    }
}
