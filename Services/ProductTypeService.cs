using Market.Entities;
using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class ProductTypeService(ApplicationDbContext dbContext) : IProductTypeService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public Page<ProductType> GetProductTypeList(SystemProductTypePage req)
        {
            var query = _dbContext.ProductTypes.AsQueryable().Where(x => x.TypeCode.Contains(req.Key) || x.TypeName.Contains(req.Key));
            var total = query.Count();
            var list = query.Skip((req.PageNumber - 1) * req.PageSize).Take(req.PageSize).ToList();
            return new Page<ProductType>
            {
                Items = list,
                Total = total,
                PageNumber = req.PageNumber,
                PageSize = req.PageSize
            };
        }
    }
}