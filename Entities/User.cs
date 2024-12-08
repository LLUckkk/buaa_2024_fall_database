using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("id")]
        [StringLength(19)]
        public string Id { get; set; }

        [Column("avatar")]
        [StringLength(255)]
        public string Avatar { get; set; }

        [Column("intro")]
        [StringLength(255)]
        public string Intro { get; set; }

        [Column("nick_name")]
        [StringLength(100)]
        public string NickName { get; set; }

        [Column("username")]
        [StringLength(100)]
        public string Username { get; set; }

        [Column("email")]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        
        [Column("student_id")]
        [StringLength(20)]
        public string StudentId { get; set; }

        [Column("password")]
        [StringLength(100)]
        public string Password { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("address")]
        [StringLength(10)]
        public string Address { get; set; }

        [Column("check_nick_name")]
        [StringLength(100)]
        public string CheckNickName { get; set; }

        [Column("check_intro")]
        [StringLength(255)]
        public string CheckIntro { get; set; }

        [Column("check_avatar")]
        [StringLength(255)]
        public string CheckAvatar { get; set; }

        [Column("check_status")]
        public int CheckStatus { get; set; }

        public ICollection<ProductInfo> ProductInfos { get; set; }
        public ICollection<ChatList> ChatListsFrom { get; set; }
        public ICollection<ChatList> ChatListsTo { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<PaymentOrder> PaymentOrders { get; set; }
        public ICollection<PaymentPay> PaymentPays { get; set; }
        public ICollection<ProductCollect> ProductCollects { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }
        public ICollection<UserAddress> UserAddresses { get; set; }
        public ICollection<VoucherOrder> VoucherOrders { get; set; }
    }
}