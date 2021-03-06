﻿using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DatingApp.API.Models
{
        public class AuthRepository : IAuthRepository
        {
            private DataContext _context;
            public AuthRepository(DataContext context)
            {
                _context = context;
            }
            public async Task<User> LogIn(string userName, string password)
            {
                var user = await _context.Users.Include(x=> x.Photos).FirstOrDefaultAsync(x => x.UserName == userName);

                if (user == null)
                    return null;

                if (!VerifyPasswordHash(password, user.PasswordSalt, user.PasswordHash))
                    return null;

                return user;
            }

            private bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
            {
                using (var hmac = new HMACSHA512(passwordSalt))
                {
                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if (computedHash[i] != passwordHash[i])
                            return false;
                    }

                    return true;
                }
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
                using (var hmac = new HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
            }

            public async Task<bool> UserExist(string userName)
            {
                return await _context.Users.AnyAsync(x => x.UserName == userName);
            }
        }
}
