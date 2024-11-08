using Core.Entities;
using Core.Repository;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecificationRepository<T> spec)
        {
            if (spec.Criteria != null){
                query = query.Where(spec.Criteria); //x => x.brand == brand
            }

            if (spec.OrderBy != null){
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDesc != null){
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            if (spec.IsDistinct){
                query = query.Distinct();
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;
        }

        public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query,
         ISpecificationRepository<T, TResult> spec){
            
            if (spec.Criteria != null){
                query = query.Where(spec.Criteria); //x => x.brand == brand
            }

            if (spec.OrderBy != null){
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDesc != null){
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            IQueryable<TResult>? selectQuery = query as IQueryable<TResult>;

            if (spec.Select != null){
                selectQuery = query.Select(spec.Select);
            }

            if (spec.IsDistinct){
                selectQuery = selectQuery?.Distinct();
            }

            if (spec.IsPagingEnabled)
            {
                selectQuery = selectQuery!.Skip(spec.Skip).Take(spec.Take);
            }

            return selectQuery ?? query.Cast<TResult>();
        }
    }
}