using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
   public interface IAuthRepository
    {
        Task<User> LogIn(string userName, string password);
        Task<User> Register(User user, string password);
        Task<bool> UserExist(string userName);

        public class AuthRepository : IAuthRepository
        {
            private DataContext _context;
            public AuthRepository(DataContext context)
            {
                _context = context;
            }
            public Task<User> LogIn(string userName, string password)
            {
                throw new NotImplementedException();
            }

            public async Task<User> Register(User user, string password)
            {
                byte[] passwordSalt, passwordHash;

                CreatePasswordHash(password, out passwordSalt, out passwordHash);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return user;
            }

            private void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
            {
                using(var hmac = new HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
            }

            public Task<bool> UserExist(string userName)
            {
                throw new NotImplementedException();
            }
        }
    }
}
