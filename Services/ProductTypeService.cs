using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class ProductTypeService(ApplicationDbContext dbContext) : IProductTypeService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public Result<Page<ProductType>> GetProductTypeList(SystemProductTypePage req)
        {
            var query = _dbContext.ProductTypes.AsQueryable();
            if (!string.IsNullOrEmpty(req.Key))
                query = query.Where(x => x.TypeCode.Contains(req.Key) || x.TypeName.Contains(req.Key));
            var total = query.Count();
            var list = query.Skip((req.PageNumber - 1) * req.PageSize).Take(req.PageSize).ToList();
            return Result<Page<ProductType>>.Ok(new Page<ProductType>
            {
                Items = list,
                Total = total,
                PageNumber = req.PageNumber,
                PageSize = req.PageSize
            });
        }

        public Result<List<ProductType>> GetList()
        {
            return Result<List<ProductType>>.Ok(_dbContext.ProductTypes.ToList());
        }

        public Result CreateProductType(ProductTypeObj req) {
            if (string.IsNullOrEmpty(req.TypeCode) || string.IsNullOrEmpty(req.TypeName))
                return Result.Fail(ResultCode.ValidateError);

            var productType = new ProductType
            {
                Id = Guid.NewGuid().ToString(),
                TypeCode = req.TypeCode,
                TypeName = req.TypeName,
                CreateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            _dbContext.ProductTypes.Add(productType);
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result.Fail(ResultCode.SaveError);
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