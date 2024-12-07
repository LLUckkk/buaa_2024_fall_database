using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("voucher_order")]
    public class VoucherOrder
    {
        [Key]
        public string? Id { get; set; }

        [Column("user_id")]
        public string? UserId { get; set; }

        [Column("voucher_id")]
        public string? VoucherId { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }
    }

}