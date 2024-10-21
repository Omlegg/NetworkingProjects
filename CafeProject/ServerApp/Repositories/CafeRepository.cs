using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServerApp.Context;
using SharedLib.Models.Entities;

namespace ServerApp.Repositories
{
    public class CafeRepository
    {
        public CafeDbContext dbContext  = new CafeDbContext();
        public Task AddUser(User user)
        {
            return Task.Run( () =>{
                dbContext.Users.Add(user);
                dbContext.Carts.Add(new Cart{User = user});
                dbContext.Logs.Add(new Log{dateTime = DateTime.Now,LogText = $"Added new User {user.Id} : {user.UserName}"});
                dbContext.SaveChanges();
            }
            );
        }

        public Task AddItem(Item item)
        {
            return Task.Run( () =>{
                dbContext.Items.Add(item);
                dbContext.Logs.Add(new Log{dateTime = DateTime.Now,LogText = $"Added new Items {item.Id} : {item.Name}"});
                dbContext.SaveChanges();
            }
            );
        }

        public Task AddCard(Card card)
        {
            return Task.Run( () =>{
                dbContext.Cards.Add(card);
                dbContext.Logs.Add(new Log{dateTime = DateTime.Now,LogText = $"Added new Items {card.Id} : {card.User.Id} user's {card.AmountOfCoffee}"});
                dbContext.SaveChanges();
            }
            );
        }

        
        public Task AddCheck(Check check)
        {
            return Task.Run( () =>{
                dbContext.Checks.Add(check);
                dbContext.Logs.Add(new Log{dateTime = DateTime.Now,LogText = $"Added new Items {check.Id} : {check.User.Id} user's"});
                dbContext.SaveChanges();
            }
            );
        }

        public Task AddCartItem(Cart cart, Item item)
        {
            return Task.Run( () =>{
                var cartItemGroup = new CartItemGroup{Item = item, Cart = cart};
                dbContext.CartItemGroup.Add(cartItemGroup);
                dbContext.Logs.Add(new Log{dateTime = DateTime.Now,LogText = $"Added new Cart Item Group {cartItemGroup.Id} : {cart.User.Id} user's {cart.Id} cart {item.Id}"});
                dbContext.SaveChanges();
            }
            );
        }

        public Task RemoveCartItem(Cart cart, Item item){
            return Task.Run( () =>{
                var cartItemGroup = dbContext.CartItemGroup.First(row => row.CartId == cart.Id && row.ItemId == item.Id);

                dbContext.Remove(cartItemGroup);
                dbContext.Logs.Add(new Log {dateTime = DateTime.Now,LogText = $"Removed Cart Item Group {cartItemGroup.Id} : {cart.User.Id} user's {cart.Id} cart {item.Id}"});
                dbContext.SaveChanges();
            });
        }
        
        public Task RemoveCartItem(CartItemGroup cartItemGroup){
            return Task.Run( () =>{
                dbContext.Remove(cartItemGroup);
                dbContext.Logs.Add(new Log {dateTime = DateTime.Now,LogText = $"Removed Cart Item Group {cartItemGroup.Id} : {cartItemGroup.Cart.User.Id} user's {cartItemGroup.Cart.Id} cart {cartItemGroup.Item.Id}"});
                dbContext.SaveChanges();
            });
        }


        public Task<User?> GetUser(string username, string password)
        {
            return Task<User?>.Run( () =>
                {
                    var user = dbContext.Users.FirstOrDefault(x => x.UserName == username && x.Password == password);
                    if(user != null){
                        dbContext.Logs.Add(new Log {dateTime = DateTime.Now,LogText = $"Selected user {user.Id} : {username} "});
                    }
                    dbContext.SaveChanges();
                    return user;
                }
            );
        }

        public Task<User?> GetUser(int id)
        {
            return Task<User?>.Run( () =>
                {
                    var user = dbContext.Users.FirstOrDefault(x => x.Id == id);
                    if(user != null){
                        dbContext.Logs.Add(new Log {dateTime = DateTime.Now,LogText = $"Selected user {user.Id} : {user.UserName} "});
                    }
                    dbContext.SaveChanges();
                    return user;
                }
            );
        }

        public Task<Cart?> GetCart(int id)
        {
            return Task<Cart?>.Run( () =>
                {
                    var cart = dbContext.Carts.FirstOrDefault(x => x.Id == id);
                    if(cart != null){
                        dbContext.Logs.Add(new Log {dateTime = DateTime.Now,LogText = $"Selected user {cart.Id} : {cart.User.Id} user's cart "});
                    }
                    dbContext.SaveChanges();
                    return cart;
                }
            );
        }

        public Task<Cart?> GetCart(User user)
        {
            return Task<Cart?>.Run( () =>
                {
                    var cart = dbContext.Carts.FirstOrDefault(x => x.User == user);
                    if(cart != null){
                        dbContext.Logs.Add(new Log {dateTime = DateTime.Now,LogText = $"Selected cart {cart.Id} : {cart.User.Id} user's cart "});
                    }
                    dbContext.SaveChanges();
                    return cart;
                }
            );
        }

        public Task<IQueryable<Card>> GetCards(User user)
        {
            return Task<IQueryable<Card>>.Run( async () =>
                {
                    var cards = dbContext.Cards.Where(x => x.User == user);
                    await dbContext.Logs.AddAsync(new Log {dateTime = DateTime.Now,LogText = $"Selected {user.Id} user's cards "});
                    dbContext.SaveChanges();
                    return cards;
                }
            );
        }

        public Task<IQueryable<Card>> GetCards(int id)
        {
            return Task<IQueryable<Card>>.Run( async () =>
                {
                    var cards = dbContext.Cards.Where(x => x.User.Id == id);
                    dbContext.Logs.AddAsync(new Log {dateTime = DateTime.Now,LogText = $"Selected {id} user's cards "});
                    dbContext.SaveChanges();
                    return cards;
                }
            );
        }

        public Task<IQueryable<CartItemGroup>?> GetCartItemGroup(Cart cart)
        {
            return Task<IQueryable<CartItemGroup>?>.Run( () =>
                {
                    var cartItemGroups = dbContext.CartItemGroup.Where(x => x.Cart == cart);
                    if(cartItemGroups != null){
                        dbContext.Logs.Add(new Log {dateTime = DateTime.Now,LogText = $"Selected cart item groups {cart.Id} user's cart "});
                    }
                    dbContext.SaveChanges();
                    return cartItemGroups;
                }
            );
        }

        public Task<Item?> GetItem(int id)
        {
            return Task<Item?>.Run( () =>
                {
                    var item = dbContext.Items.FirstOrDefault(x => x.Id == id);
                    if(item != null){
                        dbContext.Logs.Add(new Log {dateTime = DateTime.Now,LogText = $"Selected item {item.Id} : {item.Name} "});
                    }
                    dbContext.SaveChanges();
                    return item;
                }
            );
        }

        public Task<List<Item>> GetItems()
        {
            return Task<List<Item>>.Run( () =>
                {
                    var items = dbContext.Items.ToList();
                    dbContext.Logs.Add(new Log {dateTime = DateTime.Now,LogText = $"Selected all items"});
                    dbContext.SaveChanges();
                    return items;
                }
            );
        }
    }
}