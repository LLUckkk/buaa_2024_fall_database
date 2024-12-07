using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities {
    [Table("product_voucher")]
    public class ProductVoucher
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("product_id")]
        public string ProductId { get; set; }

        [ForeignKey("ProductId")]
        public ProductInfo Product { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("voucher_value")]
        public long VoucherValue { get; set; }

        [Column("stock")]
        public int Stock { get; set; }

        [Column("begin_time")]
        public DateTime BeginTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("voucher_status")]
        public int VoucherStatus { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }
    }
}