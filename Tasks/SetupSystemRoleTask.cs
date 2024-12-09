using Market.Entities;
using Market.Interfaces;
using Market.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Tasks {
    public class SetupSystemRoleTask(ILogger<SetupSystemRoleTask> logger, IServiceProvider serviceProvider) : IHostedService {
        private readonly ILogger<SetupSystemRoleTask> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SetupSystemRoleTask is starting.");

            await DoWorkAsync();

            _logger.LogInformation("SetupSystemRoleTask has completed its work.");
        }

        private Task DoWorkAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
                var systemRole = dbContext.SystemRoles.Where(r => r.RoleCode == "System").FirstOrDefault();
                if (systemRole == null)
                {
                    _logger.LogInformation("SystemRole not found, creating default SystemRole and SystemUser.");
                    var role = new SystemRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleCode = "System",
                        RoleName = "系统管理员",
                        CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds()
                    };
                    dbContext.SystemRoles.Add(role);
                    dbContext.SystemUsers.Add(new SystemUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        Username = "system",
                        Password = passwordHasher.HashPassword("system"),
                        Name = "系统管理员",
                        RoleId = role.Id,
                        RoleCode = "System",
                        RoleName = "系统管理员",
                        CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds()
                    });
                    dbContext.SaveChanges();
                }
                var adminRole = dbContext.SystemRoles.Where(r => r.RoleCode == "Admin").FirstOrDefault();
                if (adminRole == null)
                {
                    _logger.LogInformation("AdminRole not found, creating default AdminRole and AdminUser.");
                    var role = new SystemRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleCode = "Admin",
                        RoleName = "管理员",
                        CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds()
                    };
                    dbContext.SystemRoles.Add(role);
                    dbContext.SystemUsers.Add(new SystemUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        Username = "admin",
                        Password = passwordHasher.HashPassword("admin"),
                        Name = "管理员",
                        RoleId = role.Id,
                        RoleCode = "Admin",
                        RoleName = "管理员",
                        CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds()
                    });
                    dbContext.SaveChanges();
                }
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SetupSystemRoleTask is stopping.");
            return Task.CompletedTask;
        }
    }
}