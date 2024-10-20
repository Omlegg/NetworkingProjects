using System.Net;
using System.Net.Http;

const int port = 7373;

var httpListener = new HttpListener();
httpListener.Prefixes.Add($"http://*:{port}/");
httpListener.Start();
System.Console.WriteLine($"HTTP server started on http://*:{port}/");

while(true)
{
    var context = await httpListener.GetContextAsync();

    var writer = new StreamWriter(context.Response.OutputStream);
    System.Console.WriteLine(context.Request.RawUrl);

    var normalizedRawUrl = context.Request.RawUrl?.Trim().ToLower() ?? "/";
    var rawUrlItems = normalizedRawUrl.Split("/", StringSplitOptions.RemoveEmptyEntries);

    if(rawUrlItems.Length == 0 || (rawUrlItems.Length == 1 && rawUrlItems[0] == "index"))
    {
        await writer.WriteLineAsync("Show the menu\nShow your cart\nshow your cards\nbuy an item\nshow your check history\n");
        await writer.FlushAsync();

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
    else if (rawUrlItems.Length == 1)
    {
        if(rawUrlItems[0] == "login"){
            Console.WriteLine(context.Request.InputStream.EndRead);
        }
        
    }
    else{
        await writer.WriteLineAsync("404 not found");
        await writer.FlushAsync();
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
    }
}