using Microsoft.AspNetCore.Mvc;
using Repository.DTO.Response;
using Repository.Interface;

namespace eStoreAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _cateRepo;

        public CategoriesController(ICategoryRepository cateRepo)
        {
            _cateRepo = cateRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
        {
            var rs = await _cateRepo.GetCategories();
            return Ok(rs);
        }
    }
}
