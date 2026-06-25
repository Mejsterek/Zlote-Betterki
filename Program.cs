using Microsoft.AspNetCore.DataProtection;

var contentRootPath = Directory.GetCurrentDirectory();
if (contentRootPath.EndsWith("net8.0", StringComparison.OrdinalIgnoreCase) ||
    contentRootPath.EndsWith("net10.0", StringComparison.OrdinalIgnoreCase))
{
    contentRootPath = Path.GetFullPath(Path.Combine(contentRootPath, "..", "..", ".."));
}

var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = contentRootPath,
    WebRootPath = Path.Combine(contentRootPath, "wwwroot")
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

var keysPath = Path.Combine(builder.Environment.ContentRootPath, "keys");
Directory.CreateDirectory(keysPath);

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
