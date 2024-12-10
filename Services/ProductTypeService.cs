using Market.Constants;
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

        public List<ProductType> GetList()
        {
            return _dbContext.ProductTypes.ToList();
        }

        public Result CreateProductType(ProductTypeObj req) {
            if (string.IsNullOrEmpty(req.TypeCode) || string.IsNullOrEmpty(req.TypeName))
                return Result.Fail(ResultCode.ValidateError);

            var productType = new ProductType
            {
                TypeCode = req.TypeCode,
                TypeName = req.TypeName,
                CreateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            _dbContext.ProductTypes.Add(productType);
            return Result.Ok();
        }
        public Result UpdateProductType(ProductTypeObj req) {
            if (string.IsNullOrEmpty(req.TypeName))
                return Result.Fail(ResultCode.ValidateError);

            var productType = _dbContext.ProductTypes.FirstOrDefault(x => x.Id == req.Id);
            if (productType == null)
                return Result.Fail(ResultCode.NotFoundError);

            productType.TypeName = req.TypeName;
            productType.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            _dbContext.ProductTypes.Update(productType);
            _dbContext.SaveChanges();

            return Result.Ok();
        }
    }
}