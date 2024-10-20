using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Azure.Core;
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
    User? currentUser = new User();
    var context = await httpListener.GetContextAsync();

    var writer = new StreamWriter(context.Response.OutputStream);
    System.Console.WriteLine(context.Request.RawUrl);

    
    var reader = new StreamReader(context.Request.InputStream);
    
    var requestBodyStr = await reader.ReadToEndAsync();

    var normalizedRawUrl = context.Request.RawUrl?.Trim().ToLower() ?? "/";
    var rawUrlItems = normalizedRawUrl.Split("/", StringSplitOptions.RemoveEmptyEntries);

    if(rawUrlItems.Length == 0 || (rawUrlItems.Length == 1 && rawUrlItems[0] == "index"))
    {
        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
    else if (rawUrlItems.Length == 2)
    {
        if(rawUrlItems[0] == "account"){
            if(rawUrlItems[1] == "login"){
                if(context.Request.HttpMethod == "POST")
                {
                    var user = JsonSerializer.Deserialize<User>(requestBodyStr);
                    var foundUser = await repository.GetUser(user.UserName,user.Password);
                    if(foundUser != null)
                    {
                        currentUser = foundUser;
                    }
                    else{
                        await repository.AddUser(user);
                        currentUser = user;
                    }
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
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
    else if (rawUrlItems.Length == 1){
        if(rawUrlItems[0] == "menu")
        {
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
        }
        else if(rawUrlItems[0] == "cart"){
            if(context.Request.HttpMethod == "GET"){
                if(currentUser != null){
                var cart = await repository.GetCart(currentUser);
                    if(cart != null){
                        var cartItemGroups = await repository.GetCartItemGroup(cart);
                        IQueryable<Item?> items = null;
                        foreach(var cartItemGroup in cartItemGroups){
                            items = items.Append(await repository.GetItem(cartItemGroup.ItemId));
                        }
                    }
                    else{
                        context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                    }
                }else{
                    context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                }
            }
        }else{
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
    else{
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
    }
}