using AutoMapper;
using BusinessObject.Models;
using DataAccess;
using Repository.DTO.Response;
using Repository.DTO.Resquest;
using Repository.Exceptions;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implement
{
    public class ProductRepository : IProductRepository
    {
        private IMapper _mapper;

        public ProductRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ProductResponse> DeleteProduct(int productId)
        {
            try
            {
                Product? product = ProductDAO.Instance.GetAll().Where(x => x.ProductId == productId)
                                                           .SingleOrDefault();
                if (product == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found product with id!!!", productId.ToString());
                }
                await ProductDAO.Instance.DeleteProduct(product);
                return _mapper.Map<Product, ProductResponse>(product);

            }
            catch (CrudException ex)
            {
                throw new CrudException(ex.StatusCode, ex.Message, ex?.Message);
            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductResponse> GetProductById(int id)
        {
            var product = await ProductDAO.Instance.GetProduct(id);
            if(product == null)
            {
                throw new CrudException(HttpStatusCode.NotFound, "Not found product with id", id.ToString());
            }
            return _mapper.Map<Product?, ProductResponse>(product);
        }

        public async Task<IEnumerable<ProductResponse>> GetProducts()
        {
            try
            {
                var products = await ProductDAO.Instance.GetProducts();
                return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(products);
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get all products failed!", ex?.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductResponse> InsertProduct(CreateProductRequest productRequest)
        {
            try
            {

                var checkProduct = ProductDAO.Instance.GetAll().Where(x => x.ProductName.Contains(productRequest.ProductName))
                                                               .FirstOrDefault();
                if(checkProduct != null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Product is already exist!!!", productRequest.ProductName);
                }
                var product = _mapper.Map<CreateProductRequest, Product>(productRequest);
                product.ProductId = ProductDAO.Instance.GetAll().Max(x => x.ProductId) + 1;
                await ProductDAO.Instance.InsertProduct(product);
                return _mapper.Map<Product, ProductResponse>(product);
            }
            catch (CrudException ex)
            {
                throw new CrudException(ex.StatusCode, ex.Message, ex?.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductResponse> UpdateProduct(int productId, UpdateProductRequest productRequest)
        {
            try
            {
                Product? product = null;
                product = ProductDAO.Instance.GetAll().Where(x => x.ProductId == productId)
                                             .SingleOrDefault();
                if (product == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found product with id!!!", productId.ToString());
                }
                _mapper.Map<UpdateProductRequest, Product>(productRequest, product);
                await ProductDAO.Instance.UpdateProduct(product);
                return _mapper.Map<Product, ProductResponse>(product);
            }
            catch (CrudException ex)
            {
                throw new CrudException(ex.StatusCode, ex.Message, ex?.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ProductResponse>> SearchProduct(string searchString)
        {
            try
            {
                var products = ProductDAO.Instance.GetAll().Where(x => x.ProductName.Contains(searchString) || 
                                                                       x.UnitPrice.ToString().Contains(searchString)).ToList();
                                             
                if (products == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found product!!!", searchString);
                }
                return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(products);
            }
            catch (CrudException ex)
            {
                throw new CrudException(ex.StatusCode, ex.Message, ex?.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
