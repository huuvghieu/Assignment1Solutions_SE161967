using BusinessObject.Models;
using Repository.DTO.Response;
using Repository.DTO.Resquest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IProductRepository
    {
        public Task<IEnumerable<ProductResponse>> GetProducts();

        public Task<ProductResponse> GetProductById(int id);

        public Task<ProductResponse> InsertProduct(CreateProductRequest productRequest);

        public Task<ProductResponse> UpdateProduct(int productId, UpdateProductRequest productRequest);

        public Task<ProductResponse> DeleteProduct(int productId);

        public Task<IEnumerable<ProductResponse>> SearchProduct(string searchString);
    }
}
