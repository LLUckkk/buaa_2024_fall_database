using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("system_users")]
    public class SystemUser
    {
        [Key]
        [Column("id")]
        public required string Id { get; set; }

        [Column("username")]
        public required string Username { get; set; }

        [Column("password")]
        public required string Password { get; set; }

        [Column("name")]
        public required string Name { get; set; }

        [Column("role_id")]
        public required string RoleId { get; set; }

        [Column("role_code")]
        public required string RoleCode { get; set; }

        [Column("role_name")]
        public required string RoleName { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }
    }
}