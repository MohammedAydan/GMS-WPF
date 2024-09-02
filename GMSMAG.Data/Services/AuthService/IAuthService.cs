using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMSMAG.Dtos;

namespace GMSMAG.Services.AuthService
{
    public interface IAuthService
    {
        public AuthRes Login(LoginData data);
        public AuthRes Register(RegisterData data, bool firstRegister = true);
    }
}
