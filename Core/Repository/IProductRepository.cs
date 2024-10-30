using Core.Entities;

namespace Core.Repository
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(
            string? brand, string? type, string? sort);
        Task<Product?> GetProductByIdAsync(int id);
        Task<IReadOnlyList<string>> GetBrandsAsync();
        Task<IReadOnlyList<string>> GetTypesAsync();
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        bool ProductExists(int id);
        Task<bool> SaveChangesAsync();
    }
}