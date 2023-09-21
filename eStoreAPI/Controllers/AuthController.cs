using Microsoft.AspNetCore.Mvc;
using Repository.DTO.Response;
using Repository.DTO.Resquest;
using Repository.Interface;

namespace eStoreAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMemberRepository _memberRepo;

        public AuthController(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest model)
        {
            var rs = await _memberRepo.Login(model);
            return Ok(rs);
        }
    }
}
