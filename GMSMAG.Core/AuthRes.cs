using GMSMAG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSMAG.Dtos
{
    public class AuthRes
    {
        public List<string>? Erros { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? AccessToken { get; set; }
        public Admin? AdminData;
    }
}
