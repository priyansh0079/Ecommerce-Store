using System.Linq.Expressions;

namespace Core.Repository
{
    public interface ISpecificationRepository<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDesc { get; }
        Boolean IsDistinct { get; }
    }

    public interface ISpecificationRepository<T, TResult>: ISpecificationRepository<T>
    {
        Expression<Func<T, TResult>>? Select { get; }
    }
}