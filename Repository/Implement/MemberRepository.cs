using AutoMapper;
using BusinessObject.Models;
using DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.DTO.Response;
using Repository.DTO.Resquest;
using Repository.Exceptions;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implement
{
    public class MemberRepository : IMemberRepository
    {
        private IMapper _mapper;
        private string? _secretKey;
        private IConfiguration Configuration;

        public MemberRepository(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            Configuration = configuration;
        }

        public async Task<MemberReponse> DeleteMember(int id)
        {
            try
            {
                Member? member = MemberDAO.Instance.GetAll().Where(x => x.MemberId == id)
                                                           .SingleOrDefault();
                if (member == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found member with id!!!", id.ToString());
                }
                await MemberDAO.Instance.DeleteMember(member);
                return _mapper.Map<Member, MemberReponse>(member);

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

        public async Task<MemberReponse> GetMemberById(int id)
        {
            var member = await MemberDAO.Instance.GetMemberById(id);
            if (member == null)
            {
                throw new CrudException(HttpStatusCode.NotFound, "Not found member with id", id.ToString());
            }
            return _mapper.Map<Member?, MemberReponse>(member);
        }

        public async Task<IEnumerable<MemberReponse>> GetMembers()
        {
            try
            {
                var members = await MemberDAO.Instance.GetMembers();
                return _mapper.Map<IEnumerable<Member>, IEnumerable<MemberReponse>>(members);
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get all members failed!!!", ex?.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MemberReponse> InsertMember(CreateMemberRequest memberRequest)
        {
            try
            {
                var checkMember = MemberDAO.Instance.GetAll().Where(x => x.Email.Equals(memberRequest.Email))
                                                             .FirstOrDefault();
                if (checkMember != null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Member is already exits!!!", memberRequest.Email);
                }
                var member = _mapper.Map<CreateMemberRequest, Member>(memberRequest);
                member.MemberId = MemberDAO.Instance.GetAll().Max(x => x.MemberId) + 1;
                await MemberDAO.Instance.InsertMember(member);
                return _mapper.Map<Member, MemberReponse>(member);
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




        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {


            //check admin
            var admin = Configuration.GetSection("Admin");
            var emailAdmin = admin["email"];
            var passwordAdmin = admin["password"];

            bool isAdmin = loginRequest.Email.Equals(emailAdmin) && loginRequest.Password.Equals(passwordAdmin);

            if (isAdmin)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = Configuration.GetSection("ApiSetting");
                _secretKey = secretKey["Secret"];
                var key = Encoding.ASCII.GetBytes(_secretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Email, emailAdmin),
                    new Claim(ClaimTypes.Role, "Admin")
                    }),

                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                LoginResponse loginResponse = new LoginResponse
                {
                    Token = tokenHandler.WriteToken(token),
                };
                return loginResponse;
            }
            else
            {
                var member = MemberDAO.Instance.GetAll().Where(x => x.Email.Equals(loginRequest.Email) ||
                                                 x.Password.Equals(loginRequest.Password)).SingleOrDefault();

                if (member == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Member is not exits!!!", loginRequest.Email);
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = Configuration.GetSection("ApiSetting");
                _secretKey = secretKey["Secret"];
                var key = Encoding.ASCII.GetBytes(_secretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Email, member.Email),
                    new Claim(ClaimTypes.Role, "Customer")
                    }),

                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                LoginResponse loginResponse = new LoginResponse
                {
                    Token = tokenHandler.WriteToken(token),
                };
                return loginResponse;
            }


        }

        public async Task<MemberReponse> UpdateMember(int id, UpdateMemberRequest memberRequest)
        {
            try
            {
                Member? member = null;
                member = MemberDAO.Instance.GetAll().Where(x => x.MemberId == id)
                                             .SingleOrDefault();
                if (member == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found member with id!!!", id.ToString());
                }
                _mapper.Map<UpdateMemberRequest, Member>(memberRequest, member);
                await MemberDAO.Instance.UpdateMember(member);
                return _mapper.Map<Member, MemberReponse>(member);
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
