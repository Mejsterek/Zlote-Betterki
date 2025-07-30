using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Betterki.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System;

namespace Betterki.Pages
{
    public class IndexModel : PageModel
    {
        public Dictionary<string, KlipModel> Dane = new();

        public HashSet<string> KategorieZGlosami = new();
        
        [BindProperty]
        public string Haslo { get; set; }
        [BindProperty]
        public string ImieNazwisko { get; set; }

        public IActionResult OnPostZaloguj()
        {
            if (string.IsNullOrWhiteSpace(ImieNazwisko) || string.IsNullOrWhiteSpace(Haslo))
            {
                TempData["Message"] = "Uzupe³nij wszystkie pola.";
                return RedirectToPage();
            }

            var imieNazwiskoLower = ImieNazwisko.Trim().ToLowerInvariant();

            if (Haslo != "K@chamyPani¹04")
            {
                TempData["Message"] = "Niepoprawne has³o.";
                return RedirectToPage();
            }

            var sciezka = Path.Combine(Directory.GetCurrentDirectory(), "funkcjonariusze.txt");

            if (!System.IO.File.Exists(sciezka))
            {
                TempData["Message"] = "Brak listy funkcjonariuszy.";
                return RedirectToPage();
            }

            var listaFunkcjonariuszy = System.IO.File.ReadAllLines(sciezka)
                .Select(l => l.Trim().ToLowerInvariant());

            if (!listaFunkcjonariuszy.Contains(imieNazwiskoLower))
            {
                TempData["Message"] = "Nie jesteœ uprawniony do g³osowania.";
                return RedirectToPage();
            }

            HttpContext.Session.SetString("Blacha", imieNazwiskoLower);
            return RedirectToPage();
        }




        public void OnGet()
        {
            Dane = WczytajBaze();

            var user = HttpContext.Session.GetString("Blacha");

            if (!string.IsNullOrEmpty(user) && System.IO.File.Exists("glosy.txt"))
            {
                var wszystkieGlosy = System.IO.File.ReadAllLines("glosy.txt");

                foreach (var linia in wszystkieGlosy)
                {
                    var dane = linia.Split("::");
                    if (dane.Length >= 2 && dane[0] == user)
                    {
                        KategorieZGlosami.Add(dane[1]);
                    }
                }
            }


        }


        public IActionResult OnPost(string kategoria, string klip)
        {
            var blacha = HttpContext.Session.GetString("Blacha");

            if (string.IsNullOrEmpty(blacha))
            {
                TempData["Message"] = "Nie jesteœ zalogowany. Podaj blachê.";
                return RedirectToPage();
            }

            var cookieKey = $"voted_{kategoria.Replace(" ", "_")}";

            var baza = WczytajBaze();

            if (baza.ContainsKey(kategoria))
            {
                var kat = baza[kategoria];

                if (kat.Klipy != null && kat.Glosy != null && kat.Klipy.Contains(klip))
                {
                    if (!kat.Glosy.ContainsKey(klip))
                        kat.Glosy[klip] = 0;

                    kat.Glosy[klip]++;
                    ZapiszBaze(baza);

                    var rawBlacha = HttpContext.Session.GetString("Blacha");
                    if (!string.IsNullOrEmpty(rawBlacha))
                    {
                        var user = rawBlacha.Trim().ToLowerInvariant();
                        System.IO.File.AppendAllText("glosy.txt", $"{user}::{kategoria}::{klip}\n");
                    }


                    TempData["Message"] = $"Dziêkujemy za g³os w kategorii '{kategoria}'!";
                }
            }

            return Redirect($"/Index#kat_{Uri.EscapeDataString(kategoria)}");

        }

        private Dictionary<string, KlipModel> WczytajBaze()
        {
            var baza = new Dictionary<string, KlipModel>();
            var folderKlipy = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "klipy");

            if (!Directory.Exists(folderKlipy))
                return baza;

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

                if (pliki.Count > 0)
                {
                    baza[katNazwa] = new KlipModel
                    {
                        Klipy = pliki,
                        Glosy = new Dictionary<string, int>()
                    };
                }
            }

            return baza;
        }

        private void ZapiszBaze(Dictionary<string, KlipModel> baza)
        {
            var json = JsonSerializer.Serialize(baza, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText("baza.txt", json);
        }
    }
}
