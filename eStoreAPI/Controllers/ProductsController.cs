using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.DTO.Response;
using Repository.DTO.Resquest;
using Repository.Interface;

namespace eStoreAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        
        public ProductsController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        //Get: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
        {
            var rs = await _productRepo.GetProducts();
            return Ok(rs);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ProductResponse>> GetProductById([FromQuery] int id)
        {
            var rs = await _productRepo.GetProductById(id);
            return Ok(rs);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
        {
            var rs = await _productRepo.InsertProduct(request);
            return Ok(rs);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductResponse>> UpdateProduct([FromQuery] int id ,[FromBody] UpdateProductRequest request)
        {
            var rs = await _productRepo.UpdateProduct(id, request);
            return Ok(rs);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductResponse>> DeleteProduct([FromQuery] int id)
        {
            var rs = await _productRepo.DeleteProduct(id);
            return Ok(rs);
        }

        /// <summary>
        /// Search Product
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> SearchProducts([FromQuery] string searchString)
        {
            var rs = await _productRepo.SearchProduct(searchString);
            return Ok(rs);
        }
    }
}
