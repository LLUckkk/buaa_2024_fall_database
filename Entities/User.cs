using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Market.Entities
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("id")]
        [StringLength(36)]
        public required string Id { get; set; }

        [Column("avatar")]
        [StringLength(255)]
        public required string Avatar { get; set; }

        [Column("intro")]
        [StringLength(255)]
        public string? Intro { get; set; }

        [Column("nick_name")]
        [StringLength(100)]
        public required string Nickname { get; set; }

        [Column("username")]
        [StringLength(100)]
        public required string Username { get; set; }

        [Column("email")]
        [StringLength(100)]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Column("student_id")]
        [StringLength(20)]
        public required string StudentId { get; set; }

        [Column("password")]
        [StringLength(100)]
        [JsonIgnore]
        public string? Password { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("address")]
        [StringLength(10)]
        public string? Address { get; set; }

        [Column("check_nick_name")]
        [StringLength(100)]
        public string? CheckNickName { get; set; }

        [Column("check_intro")]
        [StringLength(255)]
        public string? CheckIntro { get; set; }

        [Column("check_avatar")]
        [StringLength(255)]
        public string? CheckAvatar { get; set; }

        [Column("check_status")]
        public int CheckStatus { get; set; }
    }
}