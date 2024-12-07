using Microsoft.EntityFrameworkCore;
using Market.Entities;

namespace Market
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChatList> ChatLists { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PaymentOrder> PaymentOrders { get; set; }
        public DbSet<PaymentPay> PaymentPays { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<ProductCollect> ProductCollects { get; set; }
        public DbSet<ProductInfo> ProductInfos { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductVoucher> ProductVouchers { get; set; }
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<VoucherOrder> VoucherOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}