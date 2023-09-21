using AutoMapper;
using BusinessObject.Models;
using DataAccess;
using Repository.DTO.Response;
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
    public class CategoryRepository : ICategoryRepository
    {
        private IMapper _mapper;

        public CategoryRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponse>> GetCategories()
        {
            try
            {
                var categories = await CategoryDAO.Instance.GetCategories();
                return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResponse>>(categories);
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get all categories failed!", ex?.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
