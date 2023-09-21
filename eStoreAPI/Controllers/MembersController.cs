using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.DTO.Response;
using Repository.DTO.Resquest;
using Repository.Interface;

namespace eStoreAPI.Controllers
{
    [Route("api/members")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _memberRepo;

        public MembersController(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<MemberReponse>>> GetMembers()
        {
            var rs = await _memberRepo.GetMembers();
            return Ok(rs);
        }

        [HttpGet("id")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MemberReponse>> GetMemberById([FromQuery] int id)
        {
            var rs = await _memberRepo.GetMemberById(id);
            return Ok(rs);
        }

        [HttpPost("registeration")]

        public async Task<ActionResult<MemberReponse>> RegisterMember([FromBody] CreateMemberRequest memberRequest)
        {
            var rs = await _memberRepo.InsertMember(memberRequest);
            return Ok(rs);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<MemberReponse>> UpdateMember([FromQuery] int id, [FromBody] UpdateMemberRequest memberRequest)
        {
            var rs = await _memberRepo.UpdateMember(id, memberRequest);
            return Ok(rs);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MemberReponse>> DeleteMember([FromQuery] int id)
        {
            var rs = await _memberRepo.DeleteMember(id);
            return Ok(rs);
        }

    }
}
