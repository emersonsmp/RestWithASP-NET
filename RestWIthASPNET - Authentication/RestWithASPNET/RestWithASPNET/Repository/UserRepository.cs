using Microsoft.EntityFrameworkCore;
using RestWithASPNET.data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RestWithASPNET.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MySQLContext _context;
        private DbSet<User> dataset;

        public UserRepository(MySQLContext context)
        {
            _context = context;
            dataset = _context.Set<User>();
        }

        public User? ValidateCreentials(UserVO user)
        {
            var pass = ComputeHash(user.password, SHA256.Create());
            return _context.Users.FirstOrDefault(u => (u.UserName == user.UserName) && (u.PassWord == pass));
        }

        public User? RefreshUserInfo(User user)
        {
            var all = dataset.ToList();
            var allUsers = _context.Users.ToList();
            var IsInDB = _context.Users.Any(u => u.Id == user.Id);

            if (!_context.Users.Any(u => u.Id == user.Id)) 
                return null;

            var result = _context.Users.SingleOrDefault(u => u.Id.Equals(user.Id));

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }

        private string ComputeHash(string input, HashAlgorithm algorithm)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            var builder = new StringBuilder();

            foreach(var item in hashedBytes)
            {
                builder.Append(item.ToString("x2"));
            }

            return builder.ToString();
        }

        public User? ValidateCreentials(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public bool RevokeToken(string userName)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == userName);

            if (user == null) return false;

            user.RefreshToken = null;
            _context.SaveChanges();

            return true;
        }
    }
}
