using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("product_collect")]
    public class ProductCollect
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Column("product_id")]
        public string ProductId { get; set; }

        [ForeignKey("ProductId")]
        public ProductInfo Product { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }
    }
}