using Betterki.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace Betterki.Pages;

public class AdminModel : PageModel
{
    public Dictionary<string, KlipModel> Dane { get; private set; } = new();

    public bool IsAdmin => HttpContext.Session.GetString("Admin") == "true";

    [BindProperty]
    public string AdminPassword { get; set; } = string.Empty;

    public void OnGet()
    {
        if (IsAdmin)
        {
            Dane = WczytajBaze();
        }
    }

    public IActionResult OnPostLogin()
    {
        if (AdminPassword == "TajneHaslo123")
        {
            HttpContext.Session.SetString("Admin", "true");
            return RedirectToPage();
        }

        TempData["Message"] = "Błędne hasło.";
        return RedirectToPage();
    }

    private Dictionary<string, KlipModel> WczytajBaze()
    {
        var folderKlipy = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "klipy");
        var baza = new Dictionary<string, KlipModel>();

        if (!Directory.Exists(folderKlipy))
        {
            return baza;
        }

        var glosy = new List<(string user, string kategoria, string klip)>();
        var glosyPath = Path.Combine(Directory.GetCurrentDirectory(), "glosy.txt");

        if (System.IO.File.Exists(glosyPath))
        {
            foreach (var linia in System.IO.File.ReadAllLines(glosyPath, Encoding.UTF8))
            {
                var dane = linia.Split("::");
                if (dane.Length >= 3)
                {
                    glosy.Add((dane[0], dane[1], dane[2]));
                }
            }
        }

        foreach (var katFolder in Directory.GetDirectories(folderKlipy))
        {
            var katNazwa = Path.GetFileName(katFolder);
            var pliki = Directory.GetFiles(katFolder)
                .Where(f => f.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                .Select(p => $"klipy/{katNazwa}/{Path.GetFileName(p)}")
                .ToList();

            var model = new KlipModel
            {
                Klipy = pliki,
                Glosy = new Dictionary<string, int>()
            };

            foreach (var klip in pliki)
            {
                var ile = glosy.Count(g => g.kategoria == katNazwa && g.klip == klip);
                if (ile > 0)
                {
                    model.Glosy[klip] = ile;
                }
            }

            baza[katNazwa] = model;
        }

        return baza;
    }
}
