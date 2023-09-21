using Repository.DTO.Response;
using Repository.DTO.Resquest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IMemberRepository
    {
        public Task<IEnumerable<MemberReponse>> GetMembers();
        public Task<MemberReponse> GetMemberById(int id);

        public Task<MemberReponse> InsertMember(CreateMemberRequest memberRequest);
        public Task<MemberReponse> UpdateMember(int id, UpdateMemberRequest memberRequest);

        public Task<MemberReponse> DeleteMember(int id);

        public Task<LoginResponse> Login(LoginRequest loginRequest);
    }
}
