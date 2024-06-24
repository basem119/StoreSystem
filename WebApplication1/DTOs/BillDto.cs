namespace Store_Sys.DTOs
{
    public class BillDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public ICollection<BillItemDto> BillItems { get; set; }
    }
}
