using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductDAO
    {
        private static ProductDAO _instance;
        private static readonly object _instanceLock = new object();
        private static readonly FStoreDBContext _context = new FStoreDBContext();

        private ProductDAO() { }

        public static ProductDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ProductDAO();
                    }
                    return _instance;
                }
            }
        }


        //Get all
        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                return await _context.Products.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.AsNoTracking();
        }

        //Get by id
        public async Task<Product?> GetProduct(int id)
        {
            try
            {
                var product = _context.Products.AsNoTracking().Where(p => p.ProductId == id).FirstOrDefaultAsync();
                return await product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Get by name
        public async Task<Product?> GetProductByName(string name)
        {
            try
            {
                var product = _context.Products.AsNoTracking().Where(p => p.ProductName.Contains(name)).FirstOrDefaultAsync();
                return await product;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //add product
        public async Task InsertProduct(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                _context.SaveChanges();
                _context.Entry<Product>(product).State = EntityState.Detached;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //delete product
        public async Task DeleteProduct(Product product)
        {
            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //delete product
        public async Task UpdateProduct(Product product)
        {
            try
            {
                _context.Entry<Product>(product).State = EntityState.Modified;
                _context.SaveChanges();
                _context.Entry<Product>(product).State = EntityState.Detached;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
