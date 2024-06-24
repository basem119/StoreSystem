namespace WebApplication1.Modules
{
    public class Bill
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public ICollection<BillItem> BillItems { get; set; }
    }
}
