using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSMAG.Dtos
{
    public class LoginData
    {
        public string username { get; set; }
        public string password { get; set; }

        public LoginData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
