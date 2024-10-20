using System;
using System.Net.Http;
using System.Net.Http.Json;
using SharedLib.Models.Entities;
using System.Threading.Tasks;
using System.Text.Json;

HttpClient httpClient = new HttpClient();
bool exit = false;

while (!exit)
{
    // MENU
    
    Console.WriteLine("1. Login");
    Console.WriteLine("2. Show Menu");
    Console.WriteLine("3. Show Your Cart");
    Console.WriteLine("4. Show Your Cards");
    Console.WriteLine("5. Buy Item");
    Console.WriteLine("6. Show Your Check History");
    Console.WriteLine("7. Exit");
    Console.Write("Choose an option: ");

    var choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            await Login();
            break;
        case "2":
            await ShowMenu();
            break;
        case "3":
            await ShowYourCart();
            break;
        case "4":
            await ShowYourCards();
            break;
        case "5":
            await BuyItem();
            break;
        case "6":
            await ShowYourCheckHistory();
            break;
        case "7":
            exit = true;
            break;
        default:
            Console.WriteLine("Invalid option, try again.");
            break;
    }
}

async Task Login()
{
    Console.Write("Enter username: ");
    var username = Console.ReadLine() ?? "UNKNOWN";

    Console.Write("Enter password: ");
    var password = Console.ReadLine() ?? "_";

    var loginData = new User
    {
        UserName = username,
        Password = password
    };
    var response = await httpClient.PostAsJsonAsync("http://localhost:7373/account/login", loginData);

    if (response.IsSuccessStatusCode)
    {
        var responseBodyStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Login successful: {responseBodyStr}");
    }
    else
    {
        Console.WriteLine($"Login failed. Status: {response.StatusCode}");
    }
}

async Task ShowMenu()
{
    var response = await httpClient.GetAsync("http://localhost:7373/menu");

    if (response.IsSuccessStatusCode)
    {
        var responseBodyStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Menu: {responseBodyStr}");
    }
    else
    {
        Console.WriteLine($"Error getting menu. Status: {response.StatusCode}");
    }
}

async Task ShowYourCart()
{
    var response = await httpClient.GetAsync("http://localhost:7373/cart");

    if (response.IsSuccessStatusCode)
    {
        // CART ADD HERE

        var responseBodyStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Your Cart: {responseBodyStr}");
    }
    else
    {
        Console.WriteLine($"Error getting cart. Status: {response.StatusCode}");
    }
}

async Task ShowYourCards()
{
    HttpResponseMessage response = await httpClient.GetAsync("http://localhost:7373/cards");

    if (response.IsSuccessStatusCode)
    {

        // CARDS ADD HERE

        var responseBodyStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Your Cards: {responseBodyStr}");
    }
    else
    {
        Console.WriteLine($"Error getting cards. Status: {response.StatusCode}");
    }
}

async Task BuyItem()
{
    Console.Write("Enter item ID to buy: ");
    var itemId = Console.ReadLine();

    var response = await httpClient.PostAsJsonAsync($"http://localhost:7373/buy/{itemId}", new { });

    if (response.IsSuccessStatusCode)
    {
        // BUY ADD HERE

        var responseBodyStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Item purchased: {responseBodyStr}");
    }
    else
    {
        Console.WriteLine($"Error purchasing item. Status: {response.StatusCode}");
    }
}

async Task ShowYourCheckHistory()
{
    var response = await httpClient.GetAsync("http://localhost:7373/checks");

    if (response.IsSuccessStatusCode)
    {
        // CHECK HISTORY ADD HERE

        var responseBodyStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Your Check History: {responseBodyStr}");
    }
    else
    {
        Console.WriteLine($"Error getting check history. Status: {response.StatusCode}");
    }
}