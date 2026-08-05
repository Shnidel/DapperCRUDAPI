using DapperCRUDAPI.Models;

namespace DapperCRUDAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<int> InsertAsync(Product product);
        Task<int> UpdateAsync(Product product);
        Task<int> DeleteAsync(int productId);
    }
}
