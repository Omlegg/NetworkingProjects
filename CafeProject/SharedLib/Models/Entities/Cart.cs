using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace SharedLib.Models.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        public User User {get; set;}

        public Collection<CartItemGroup> CartItemGroups { get;}
    }
}