namespace Market.Models
{
    public class ProductVoucherCreate
    {
        public string? ProductId { get; set; }
        public double VoucherValue { get; set; }
        public long BeginTime { get; set; }
        public long EndTime { get; set; }
    }
}