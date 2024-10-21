using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Azure;
using ServerApp.Context;
using ServerApp.Repositories;
using SharedLib.Models.Entities;

const int port = 7373;

var httpListener = new HttpListener();
httpListener.Prefixes.Add($"http://*:{port}/");
httpListener.Start();
System.Console.WriteLine($"HTTP server started on http://127.0.0.1:{port}/");
var repository = new CafeRepository();
var dbContext = new CafeDbContext();
dbContext.Database.EnsureCreated();

while(true)
{
    var context = await httpListener.GetContextAsync();

    var writer = new StreamWriter(context.Response.OutputStream);
    System.Console.WriteLine(context.Request.RawUrl);

    
    var reader = new StreamReader(context.Request.InputStream);
    
    var requestBodyStr = await reader.ReadToEndAsync();

    var normalizedRawUrl = context.Request.RawUrl?.Trim().ToLower() ?? "/";
    var rawUrlItems = normalizedRawUrl.Split("/", StringSplitOptions.RemoveEmptyEntries);

    if ((rawUrlItems.Length == 1  && rawUrlItems[0] == "menu")|| (rawUrlItems.Length == 0 || (rawUrlItems.Length == 1 && rawUrlItems[0] == "index"))){
            await Task.Run( async () => {
            if(context.Request.HttpMethod == "GET")
            {
                var items = await repository.GetItems();
                await writer.WriteAsync(JsonSerializer.Serialize(items));
                await writer.FlushAsync();
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
            });
    }
    else if (rawUrlItems.Length == 2){
        if(rawUrlItems[0] == "cards"){
            if(context.Request.HttpMethod == "GET"){
                await Task.Run( async () => {
                if(int.TryParse(rawUrlItems[1], out int userId)){
                    var cards = await repository.GetCards(userId);
                    await writer.WriteAsync(JsonSerializer.Serialize(cards));
                    await writer.FlushAsync();
                }
                else{
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                });
            }else{
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
        }
        else if(rawUrlItems[0] == "cart"){
            if(context.Request.HttpMethod == "GET"){
                if(int.TryParse(rawUrlItems[1], out int userId)){
                    await Task.Run( async () => {
                        var cart = await repository.GetCart(userId);
                        if(cart != null){
                            var cartItemGroups = await repository.GetCartItemGroup(cart);
                            IQueryable<Item?> items = null;
                            foreach(var cartItemGroup in cartItemGroups){
                                items = items.Append(await repository.GetItem(cartItemGroup.ItemId));
                            }
                        }
                        else{
                            context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                        }
                    });
                }else{
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
        }
        else if(rawUrlItems[0] == "account"){
            if(rawUrlItems[1] == "login"){
                if(context.Request.HttpMethod == "POST")
                {
                    await Task.Run(async () => {
                        Console.WriteLine(requestBodyStr);
                        var user = JsonSerializer.Deserialize<User>(requestBodyStr);
                        var foundUser = await repository.GetUser(user.userName,user.password);
                        if(foundUser  == null){
                            await repository.AddUser(user);
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                        }
                        else{
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                        }

                    });
                }
                else{
                    context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                }
            }
            else{
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
        else{
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
    else if (rawUrlItems.Length == 3)
    {
        if(rawUrlItems[0] == "buy"){
            if(context.Request.HttpMethod == "DELETE"){
                await Task.Run(async () => {
                    var user = await repository.GetUser(rawUrlItems[1], rawUrlItems[2]);
                    var cart  = await repository.GetCart(user);
                    if(cart != null){
                        var cartItemGroups = await repository.GetCartItemGroup(cart);
                        decimal total_price = 0;
                        foreach(var cartItemGroup in cartItemGroups){
                            total_price += cartItemGroup.Item.Price;
                            await repository.RemoveCartItem(cartItemGroup);
                            await repository.AddCheck(new Check{ Price = total_price, User = new User {id = user.id}});
                        }
                    }
                    else{
                        context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                    }
                });
            }
        }
    }
    else if (rawUrlItems.Length == 4){
        if(rawUrlItems[0] == "buy"){
            if(context.Request.HttpMethod == "PUT"){
                if(int.TryParse(rawUrlItems[1], out int itemId) ){
                    await Task.Run( async () => {
                        string userName = rawUrlItems[2];
                        string userPassword = rawUrlItems[3];
                        var item  = await repository.GetItem(itemId);
                        var cart  = await repository.GetCart(userName,userPassword);
                        if(item != null && cart != null){
                            await repository.AddCartItem(cart,item);
                        }
                        else{
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        }
                    });
                }else{
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else{
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
        }
        else{
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
    else{
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
    }

    
    context.Response.Close();
}