using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDAO
    {
        private static CategoryDAO _instance;
        private static readonly object _instanceLock = new object();
        private static readonly FStoreDBContext _context = new FStoreDBContext();

        private CategoryDAO()
        {

        }

        public static CategoryDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new CategoryDAO();
                    }
                    return _instance;
                }
            }
        }

        //get all
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();

        }
    }
}
