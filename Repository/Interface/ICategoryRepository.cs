using Repository.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<CategoryResponse>> GetCategories();
    }
}
