using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("payment_type")]
    public class PaymentType
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("type_name")]
        [StringLength(200)]
        public required string TypeName { get; set; }

        [Column("wx_pay")]
        public decimal WxPay { get; set; }

        [Column("ali_pay")]
        public decimal AliPay { get; set; }
    }
}