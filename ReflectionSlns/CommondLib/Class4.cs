using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommondLib
{
    public class Class4
    {
        private static string A { get; set; }
        public string B;
        public string C { get; set; }

        [Required]
        public int Id { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        static Class4()
        {
            A = "666";
        }

        public Class4()
        {
            B = "666";
        }

        public Class4(string message)
        {
            C = message;
        }

        public string Add(string a, string b)
        {
            return a + b;
        }
    }
}
