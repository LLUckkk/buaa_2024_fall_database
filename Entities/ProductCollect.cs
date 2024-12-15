using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("product_collect")]
    public class ProductCollect
    {
        [Key]
        [Column("id")]
        public required string Id { get; set; }

        [Column("user_id")]
        public required string UserId { get; set; }

        [Column("product_id")]
        public required string ProductId { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }
    }
}