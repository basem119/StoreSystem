using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_Sys.DTOs;
using WebApplication1.Modules;
using WebApplication1.UnitOfWork;

namespace Store_Sys.Controllers
{
    [ApiController]
    [Route("api/bills")]
    public class BillsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;


        public BillsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillDto>>> GetBills()
        {
            var bills = await _unitOfWork.Bills.GetAllWithDetailsAsync(b => b.BillItems);
            if (bills == null)
            {
                return NotFound();
            }

            var billDtos = bills.Select(bill => new BillDto
            {
                Id = bill.Id,
                CustomerId = bill.CustomerId,
                TotalAmount = bill.TotalAmount,
                Date = bill.Date,
                BillItems = bill.BillItems.Select(bi => new BillItemDto
                {
                    Id = bi.Id,
                    BillId = bi.BillId,
                    ProductId = bi.ProductId,
                    Quantity = bi.Quantity,
                    Price = bi.Price
                }).ToList()
            });
            return Ok(billDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BillDto>> GetBill(int id)
        {
            var bill = await _unitOfWork.Bills.GetByIdWithDetailsAsync(id, b => b.BillItems);
            if (bill == null)
            {
                return NotFound();
            }

            var billDto = new BillDto
            {
                Id = bill.Id,
                CustomerId = bill.CustomerId,
                TotalAmount = bill.TotalAmount,
                Date = bill.Date,
                BillItems = bill.BillItems.Select(bi => new BillItemDto
                {
                    Id = bi.Id,
                    BillId = bi.BillId,
                    ProductId = bi.ProductId,
                    Quantity = bi.Quantity,
                    Price = bi.Price
                }).ToList()
            };
            return Ok(billDto);
        }

        [HttpPost]
        public async Task<ActionResult<BillDto>> PostBill(BillDto billDto)
        {
            var bill = new Bill
            {
                CustomerId = billDto.CustomerId,
                TotalAmount = billDto.TotalAmount,
                Date = billDto.Date,
                BillItems = billDto.BillItems.Select(bi => new BillItem
                {
                    ProductId = bi.ProductId,
                    Quantity = bi.Quantity,
                    Price = bi.Price
                }).ToList()
            };

            await _unitOfWork.Bills.AddAsync(bill);
            await _unitOfWork.CompleteAsync();

            billDto.Id = bill.Id;
            return CreatedAtAction(nameof(GetBill), new { id = bill.Id }, billDto);
        }
    }

}
