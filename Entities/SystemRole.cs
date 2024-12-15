using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("system_role")]
    public class SystemRole
    {
        [Key]
        [Column("id", TypeName = "varchar(36)")]
        public required string Id { get; set; }

        [Column("role_code", TypeName = "varchar(20)")]
        public required string RoleCode { get; set; }

        [Column("role_name", TypeName = "varchar(20)")]
        public required string RoleName { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }
    }
}