using System.IO;
using Microsoft.AspNetCore.DataProtection;

var contentRootPath = Directory.GetCurrentDirectory();

if (contentRootPath.EndsWith("net8.0", StringComparison.OrdinalIgnoreCase))
{
    contentRootPath = Path.GetFullPath(Path.Combine(contentRootPath, "..", "..", ".."));
}

var webRootPath = Path.Combine(contentRootPath, "wwwroot");

var options = new WebApplicationOptions
{
    Args = args,
    WebRootPath = webRootPath,
    ContentRootPath = contentRootPath
};

var builder = WebApplication.CreateBuilder(options);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages();

builder.Services.AddAuthorization();

var keysPath = Path.Combine(Directory.GetCurrentDirectory(), "keys");
if (!Directory.Exists(keysPath))
{
    Directory.CreateDirectory(keysPath);
}

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("BetterkiApp");

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
