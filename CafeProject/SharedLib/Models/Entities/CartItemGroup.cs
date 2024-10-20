using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLib.Models.Entities
{
    public class CartItemGroup
    {
        public int Id {get;set;}

        public Item Item {get;set;}
        public Cart Cart {get;set;}

        public int ItemId {get;set;}

        public int CartId {get;set;}
    }
}