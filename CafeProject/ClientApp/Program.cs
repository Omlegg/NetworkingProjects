using System;
using System.Net.Http;
using System.Net.Http.Json;
using SharedLib.Models.Entities;
using System.Threading.Tasks;

HttpClient httpClient = new HttpClient();
bool exit = false;
bool isLoggedIn = false;

if (!await CheckServerConnection())
{
    Console.WriteLine("Unable to connect to the server. Program will now exit.");
    return;
}

while (!exit)
{
    if (!isLoggedIn)
    {
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Exit");
        Console.Write("Choose an option: ");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                isLoggedIn = await Login();
                if (!isLoggedIn)
                {
                    Console.WriteLine("Login failed. Try again.");
                }
                break;
            case "2":
                exit = true;
                break;
            default:
                Console.WriteLine("Invalid option, try again.");
                break;
        }
    }
    else
    {
        Console.WriteLine("1. Show Menu");
        Console.WriteLine("2. Show Your Cart");
        Console.WriteLine("3. Show Your Cards");
        Console.WriteLine("4. Add to Cart");
        Console.WriteLine("5. Purchase all");
        Console.WriteLine("6. Show Your Check History");
        Console.WriteLine("7. Logout");
        Console.WriteLine("8. Exit");
        Console.Write("Choose an option: ");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                await ShowMenu();
                break;
            case "2":
                await ShowYourCart();
                break;
            case "3":
                await ShowYourCards();
                break;
            case "4":
                await AddToCart();
                break;
            case "5":
                await Purchase();
                break;
            case "6":
                await ShowYourCheckHistory();
                break;
            case "7":
                isLoggedIn = false; // Added Logout
                break;
            case "8":
                exit = true;
                break;
            default:
                Console.WriteLine("Invalid option, try again.");
                break;
        }
    }
}

async Task<bool> CheckServerConnection()
{
    try
    {
        var response = await httpClient.GetAsync("http://localhost:7373");
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Connected to the server.");
            return true;
        }
        else
        {
            Console.WriteLine("Error connecting to the server.");
            return false;
        }
    }
    catch (HttpRequestException)
    {
        Console.WriteLine("Error: Server is not reachable.");
        return false;
    }
}

async Task<bool> Login()         // returns true if success / false if login failed
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
        return true;
    }
    else
    {
        Console.WriteLine($"Login failed. Status: {response.StatusCode}");
        return false;
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
    var response = await httpClient.GetAsync("http://localhost:7373/cards");

    if (response.IsSuccessStatusCode)
    {
        var responseBodyStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Your Cards: {responseBodyStr}");
    }
    else
    {
        Console.WriteLine($"Error getting cards. Status: {response.StatusCode}");
    }
}

async Task AddToCart()
{
    Console.Write("Enter item ID to add to cart: ");
    var itemId = Console.ReadLine();

    var response = await httpClient.PostAsJsonAsync($"http://localhost:7373/cart/add/{itemId}", new { });

    if (response.IsSuccessStatusCode)
    {
        var responseBodyStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Item added to cart: {responseBodyStr}");
    }
    else
    {
        Console.WriteLine($"Error adding to cart. Status: {response.StatusCode}");
    }
}
async Task Purchase()
{
    var response = await httpClient.PostAsync($"http://localhost:7373/cart/purchase", null);
    if (response.IsSuccessStatusCode)
    {
        var responseBodyStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Purchased all: {responseBodyStr}");
    }
    else
    {
        Console.WriteLine($"Error purchasing items. Status: {response.StatusCode}");
    }
}

async Task ShowYourCheckHistory()
{
    var response = await httpClient.GetAsync("http://localhost:7373/checks");

    if (response.IsSuccessStatusCode)
    {
        var responseBodyStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Your Check History: {responseBodyStr}");
    }
    else
    {
        Console.WriteLine($"Error getting check history. Status: {response.StatusCode}");
    }
}