using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMSMAG.Core.Data;
using GMSMAG.Dtos;

namespace GMSMAG.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private AppDbContext dbContext;

        public AuthService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public AuthRes Login(LoginData data)
        {
            var user = dbContext.Admins.FirstOrDefault(u => u.Username == data.username);

            if (user == null)
            {
                return new AuthRes
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(data.password, user.Password))
            {
                return new AuthRes
                {
                    Success = false,
                    Message = "Invalid password"
                };
            }

            return new AuthRes
            {
                Success = true,
                Message = "Login successful",
                AdminData = user
            };
        }

        public AuthRes Register(RegisterData data, bool firstRegister = true)
        {
            if (firstRegister.Equals(true))
            {
                if (data.Password != data.ConfirmPassword)
                {
                    return new AuthRes
                    {
                        Success = false,
                        Message = "Password and Confirm Password do not match"
                    };
                }
            }

            data.Password = BCrypt.Net.BCrypt.HashPassword(data.Password);

            dbContext.Admins.Add(data);
            dbContext.SaveChanges();

            var admin = dbContext.Admins.FirstOrDefault(
                a => a.Username == data.Username && a.Email == data.Email && a.Password == data.Password
                );


            return new AuthRes
            {
                Success = true,
                Message = "User registered successfully",
                AdminData = admin
            };
        }
    }
}
