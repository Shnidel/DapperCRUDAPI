using Dapper;
using DapperCRUDAPI.Models;
using System.Data;

namespace DapperCRUDAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _connection;

        public ProductRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> DeleteAsync(int productId)
        {
            var sql = @"
                        DELETE FROM SalesLT.Product
                        WHERE ProductId = @ProductId;       
                       ";

            await _connection.ExecuteAsync(sql, new { ProductId = productId });
            return productId;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var sql = @"
                        Select * From SalesLT.Product;
                       ";

            return await _connection.QueryAsync<Product>(sql);
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var sql = @"
                        Select * From SalesLT.Product Where ProductID = @Id;
                       ";

            return _connection.QuerySingleAsync<Product>(sql, new { Id = id });
        }

        public async Task<int> InsertAsync(Product product)
        {
            var sql = @"
                        INSERT INTO SalesLT.Product (
                            ProductId,
                            Name,
                            ProductNumber,
                            Color,
                            StandardCost,
                            ListPrice,
                            Size,
                            Weight,
                            ProductCategoryId,
                            ProductModelId,
                            SellStartDate,
                            SellEndDate,
                            DiscontinuedDate,
                            ThumbNailPhoto,
                            ThumbnailPhotoFileName,
                            rowguid,
                            ModifiedDate
                        ) VALUES (
                            @ProductId,
                            @Name,
                            @ProductNumber,
                            @Color,
                            @StandardCost,
                            @ListPrice,
                            @Size,
                            @Weight,
                            @ProductCategoryId,
                            @ProductModelId,
                            @SellStartDate,
                            @SellEndDate,
                            @DiscontinuedDate,
                            @ThumbNailPhoto,
                            @ThumbnailPhotoFileName,
                            @rowguid,
                            @ModifiedDate
                        )
                        SELECT CAST(SCOPE_IDENTITY() AS INT);
                    ";

            var productID = await _connection.ExecuteScalarAsync<int>(sql, product);
            return productID;
        }

        public async Task<int> UpdateAsync(Product product)
        {
            var sql = @"UPDATE SalesLT.Product
                        SET
                            Name = @Name,
                            ProductNumber = @ProductNumber,
                            Color = @Color,
                            StandardCost = @StandardCost,
                            ListPrice = @ListPrice,
                            Size = @Size,
                            Weight = @Weight,
                            ProductCategoryId = @ProductCategoryId,
                            ProductModelId = @ProductModelId,
                            SellStartDate = @SellStartDate,
                            SellEndDate = @SellEndDate,
                            DiscontinuedDate = @DiscontinuedDate,
                            ThumbNailPhoto = @ThumbNailPhoto,
                            ThumbnailPhotoFileName = @ThumbnailPhotoFileName,
                            rowguid = @rowguid,
                            ModifiedDate = @ModifiedDate
                        WHERE ProductId = @ProductId;
                    ";
            
            return await _connection.ExecuteAsync(sql, product);
        }
    }
}
