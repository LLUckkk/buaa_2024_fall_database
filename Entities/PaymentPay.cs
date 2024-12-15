using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("payment_pay")]
    public class PaymentPay
    {
        [Key]
        [Column("id")]
        public required string Id { get; set; }

        [Column("user_id")]
        public required string UserId { get; set; }

        [Column("order_id")]
        public required string OrderId { get; set; }

        [Column("payment_price")]
        public long PaymentPrice { get; set; }

        [Column("payment_type")]
        public required string PaymentType { get; set; }

        [Column("payment_result_data")]
        public string? PaymentResultData { get; set; }

        [Column("payment_time_start")]
        public required string PaymentTimeStart { get; set; }

        [Column("payment_time_expire")]
        public required string PaymentTimeExpire { get; set; }

        [Column("payment_status")]
        public int PaymentStatus { get; set; } = 0;

        [Column("process_status")]
        public int ProcessStatus { get; set; } = 0;

        [Column("time_create")]
        public DateTime TimeCreate { get; set; }

        [Column("time_update")]
        public DateTime TimeUpdate { get; set; }

        [Column("time_finish")]
        public DateTime? TimeFinish { get; set; }
    }
}