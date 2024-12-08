using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("user_address")]
    public class UserAddress
    {
        [Key]
        [Column("id")]
        [StringLength(36)]
        public string Id { get; set; }

        [Column("user_id")]
        [StringLength(36)]
        public string UserId { get; set; }

        [Column("name")]
        [StringLength(10)]
        public string Name { get; set; }

        [Column("phone")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Column("address")]
        [StringLength(255)]
        public string Address { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }
    }
}