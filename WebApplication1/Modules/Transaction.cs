using WebApplication1.Modules;

namespace Store_Sys.Modules
{
    public class Transaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } // "Deposit" or "Payment"
    }
}
