using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("system_user")]
    public class SystemUser
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("role_id")]
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public SystemRole Role { get; set; }

        [Column("role_code")]
        public string RoleCode { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }
    }
}