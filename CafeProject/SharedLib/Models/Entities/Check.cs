using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLib.Models.Entities
{
    public class Check
    {
        public int Id { get; set; }

        public decimal Price {get;set;}

        public User User {get;set;}
    }
}