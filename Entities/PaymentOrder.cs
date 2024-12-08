using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    public class PaymentOrder
    {
        [Key]
        [Column(TypeName = "varchar(36)")]
        public string Id { get; set; }

        [Column(TypeName = "varchar(32)")]
        public string OrderNumber { get; set; }

        [Column(TypeName = "varchar(36)")]
        public string UserId { get; set; }

        [Column(TypeName = "bigint")]
        public long PayPrice { get; set; }

        [Column(TypeName = "bigint")]
        public long PayTypeId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string PayTypeName { get; set; }

        [Column(TypeName = "int")]
        public int OrderStatus { get; set; }

        [Column(TypeName = "varchar(36)")]
        public string PaymentPayId { get; set; }

        [Column(TypeName = "int")]
        public int PaymentStatus { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string PaymentType { get; set; }

        [Column(TypeName = "int")]
        public int ProcessStatus { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime TimeCreate { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime TimeUpdate { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime TimeFinish { get; set; }

    }
}