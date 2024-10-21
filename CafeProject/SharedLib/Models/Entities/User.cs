using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLib.Models.Entities
{
    public class User
    {
        public int id { get; set; }

        public string userName { get; set; }

        public string password { get; set; }
    }
}