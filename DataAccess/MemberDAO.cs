using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MemberDAO
    {
        private static MemberDAO _instance;
        private static readonly object _instanceLock = new object();
        private static readonly FStoreDBContext _context = new FStoreDBContext();

        private MemberDAO()
        {

        }

        public static MemberDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if(_instance == null)
                    {
                        _instance = new MemberDAO();
                    }
                    return _instance;
                }
            }
        }

        //get all
        public async Task<IEnumerable<Member>> GetMembers()
        {
            try
            {
                return await _context.Members.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Member> GetAll()
        {
            try
            {
                return _context.Members.AsNoTracking();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //get email and password
        public async Task<Member?> GetMemberByEmailandPass(string email, string password)
        {
            try
            {
                return await _context.Members.AsNoTracking().Where(x => x.Email.Equals(email) &&
                                                                        x.Password.Equals(password))
                                                            .SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //get by id
        public async Task<Member?> GetMemberById(int id)
        {
            try
            {
                return await _context.Members.AsNoTracking().Where(x => x.MemberId == id).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //create
        public async Task InsertMember(Member member)
        {
            try
            {
                await _context.Members.AddAsync(member);
                _context.SaveChanges();
                _context.Entry<Member>(member).State = EntityState.Detached;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        //update
        public async Task UpdateMember(Member member)
        {
            try
            {
                _context.Entry<Member>(member).State = EntityState.Modified;
                _context.SaveChanges();
                _context.Entry<Member>(member).State = EntityState.Detached;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //delete
        public async Task DeleteMember(Member member)
        {
            try
            {
                _context.Members.Remove(member);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
    }
}
