using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("system_role")]
    public class SystemRole
    {
        [Key]
        [Column("id", TypeName = "varchar(19)")]
        public string Id { get; set; }

        [Column("role_code", TypeName = "varchar(20)")]
        public string RoleCode { get; set; }

        [Column("role_name", TypeName = "varchar(20)")]
        public string RoleName { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }

        public ICollection<SystemUser> SystemUsers { get; set; }
    }
}