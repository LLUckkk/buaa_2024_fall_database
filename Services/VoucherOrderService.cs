using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class VoucherOrderService(ITokenService tokenService, ApplicationDbContext dbcontext) : IVoucherOrderService {
        private readonly ApplicationDbContext _dbContext = dbcontext;
        private readonly ITokenService _tokenService = tokenService;

        public Result CreateTimeLimitedOfferOrder(string voucherId)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}