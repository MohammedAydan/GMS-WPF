using GMSMAG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSMAG.Dtos
{
    public class RegisterData:Admin
    {
        public string ConfirmPassword { get; set; }
    }
}
