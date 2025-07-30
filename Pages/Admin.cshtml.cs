using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Betterki.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System;
namespace Betterki.Pages
{
    public class AdminModel : PageModel
    {
        public Dictionary<string, KlipModel> Dane = new();

        public bool IsAdmin => HttpContext.Session.GetString("Admin") == "true";

        [BindProperty]
        public string AdminPassword { get; set; }

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

            TempData["Message"] = "B³êdne has³o.";
            return RedirectToPage();
        }

        private Dictionary<string, KlipModel> WczytajBaze()
        {
            var folderKlipy = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "klipy");
            var baza = new Dictionary<string, KlipModel>();

            if (!Directory.Exists(folderKlipy))
                return baza;

            var glosy = new List<(string user, string kategoria, string klip)>();
            var glosyPath = Path.Combine(Directory.GetCurrentDirectory(), "glosy.txt");

            if (System.IO.File.Exists(glosyPath))
            {
                foreach (var linia in System.IO.File.ReadAllLines(glosyPath))
                {
                    var dane = linia.Split("::");
                    if (dane.Length >= 3)
                    {
                        glosy.Add((dane[0], dane[1], dane[2]));
                    }
                }
            }

            var kategorie = Directory.GetDirectories(folderKlipy);
            foreach (var katFolder in kategorie)
            {
                var katNazwa = Path.GetFileName(katFolder);
                var pliki = Directory.GetFiles(katFolder)
                    .Where(f => f.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) ||
                                f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
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
                        model.Glosy[klip] = ile;
                }

                baza[katNazwa] = model;
            }

            return baza;
        }

    }
}
