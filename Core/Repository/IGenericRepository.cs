using Core.Entities;

namespace Core.Repository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T?> GetEntityWithSpec(ISpecificationRepository<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecificationRepository<T> spec);
        Task<TResult?> GetEntityWithSpec<TResult>(ISpecificationRepository<T, TResult> spec);
        Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecificationRepository<T, TResult> spec);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<bool> SaveChangesAsync();
        bool EntityExists(int id);
    }
}