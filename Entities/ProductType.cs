using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("product_type")]
    public class ProductType
    {
        [Key]
        [Column("id", TypeName = "VARCHAR(36)")]
        public required string Id { get; set; }

        [Column("type_code", TypeName = "VARCHAR(10)")]
        public required string TypeCode { get; set; }

        [Column("type_name", TypeName = "VARCHAR(20)")]
        public required string TypeName { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }
    }
}