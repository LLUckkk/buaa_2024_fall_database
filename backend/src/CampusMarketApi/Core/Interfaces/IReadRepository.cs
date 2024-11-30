using Ardalis.Specification;

namespace CampusMarketApi.Core.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}