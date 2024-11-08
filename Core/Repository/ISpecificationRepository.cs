using System.Linq.Expressions;

namespace Core.Repository
{
    public interface ISpecificationRepository<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDesc { get; }
        Boolean IsDistinct { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
        IQueryable<T> ApplyCriteria(IQueryable<T> query);
    }

    public interface ISpecificationRepository<T, TResult>: ISpecificationRepository<T>
    {
        Expression<Func<T, TResult>>? Select { get; }
    }
}