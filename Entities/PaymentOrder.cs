using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("payment_order")]
    public class PaymentOrder
    {
        [Key]
        [Column("id", TypeName = "varchar(36)")]
        public required string Id { get; set; }

        [Column("order_number", TypeName = "varchar(32)")]
        public required string OrderNumber { get; set; }

        [Column("user_id", TypeName = "varchar(36)")]
        public required string UserId { get; set; }

        [Column("pay_price", TypeName = "bigint")]
        public long PayPrice { get; set; }

        [Column("pay_type_id", TypeName = "bigint")]
        public long PayTypeId { get; set; }

        [Column("pay_type_name", TypeName = "varchar(200)")]
        public string? PayTypeName { get; set; }

        [Column("order_status", TypeName = "int")]
        public int OrderStatus { get; set; }

        [Column("payment_pay_id", TypeName = "varchar(36)")]
        public string? PaymentPayId { get; set; }

        [Column("payment_status", TypeName = "int")]
        public int PaymentStatus { get; set; }

        [Column("payment_type", TypeName = "varchar(20)")]
        public string? PaymentType { get; set; }

        [Column("process_status", TypeName = "int")]
        public int ProcessStatus { get; set; }

        [Column("time_create")]
        public DateTime TimeCreate { get; set; }

        [Column("time_update")]
        public DateTime TimeUpdate { get; set; }

        [Column("time_finish")]
        public DateTime TimeFinish { get; set; }
    }
}
