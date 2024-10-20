using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLib.Models.Entities
{
    public class Card
    {
        public int Id { get; set; }

        public int AmountOfCoffee {get;set;}

        public User User {get;set;}
    }
}