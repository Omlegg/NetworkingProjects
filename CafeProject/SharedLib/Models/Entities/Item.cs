using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLib.Models.Entities
{
    public class Item
    {
        public int Id { get; set;}

        public string Name { get; set;}

        public decimal Price { get; set;}

        public ICollection<CartItemGroup> Carts { get;}
    }
}