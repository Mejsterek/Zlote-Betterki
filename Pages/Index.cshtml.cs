using System.Text;
using Betterki.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Betterki.Pages;

public class IndexModel : PageModel
{
    public Dictionary<string, KlipModel> Dane { get; private set; } = new();
    public HashSet<string> KategorieZGlosami { get; private set; } = new();

    [BindProperty]
    public string Haslo { get; set; } = string.Empty;

    [BindProperty]
    public string ImieNazwisko { get; set; } = string.Empty;

    public IActionResult OnPostZaloguj()
    {
        if (string.IsNullOrWhiteSpace(ImieNazwisko) || string.IsNullOrWhiteSpace(Haslo))
        {
            TempData["Message"] = "Uzupełnij wszystkie pola.";
            return RedirectToPage();
        }

        if (Haslo != "K@chamyPanią04")
        {
            TempData["Message"] = "Niepoprawne hasło.";
            return RedirectToPage();
        }

        var sciezka = Path.Combine(Directory.GetCurrentDirectory(), "funkcjonariusze.txt");
        if (!System.IO.File.Exists(sciezka))
        {
            TempData["Message"] = "Brak listy funkcjonariuszy.";
            return RedirectToPage();
        }

        var imieNazwiskoLower = ImieNazwisko.Trim().ToLowerInvariant();
        var listaFunkcjonariuszy = System.IO.File.ReadAllLines(sciezka, Encoding.UTF8)
            .Select(l => l.Trim().ToLowerInvariant());

        if (!listaFunkcjonariuszy.Contains(imieNazwiskoLower))
        {
            TempData["Message"] = "Nie jesteś uprawniony do głosowania.";
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
            var wszystkieGlosy = System.IO.File.ReadAllLines("glosy.txt", Encoding.UTF8);
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
            TempData["Message"] = "Nie jesteś zalogowany. Podaj blachę.";
            return RedirectToPage();
        }

        var baza = WczytajBaze();
        if (baza.TryGetValue(kategoria, out var kat) && kat.Klipy.Contains(klip))
        {
            kat.Glosy[klip] = kat.Glosy.GetValueOrDefault(klip) + 1;
            ZapiszBaze(baza);

            System.IO.File.AppendAllText(
                "glosy.txt",
                $"{blacha.Trim().ToLowerInvariant()}::{kategoria}::{klip}{Environment.NewLine}",
                Encoding.UTF8);

            TempData["Message"] = $"Dziękujemy za głos w kategorii '{kategoria}'!";
        }

        return Redirect($"/Index#kat_{Uri.EscapeDataString(kategoria)}");
    }

    private Dictionary<string, KlipModel> WczytajBaze()
    {
        var baza = new Dictionary<string, KlipModel>();
        var folderKlipy = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "klipy");

        if (!Directory.Exists(folderKlipy))
        {
            return baza;
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

            if (pliki.Count == 0)
            {
                continue;
            }

            baza[katNazwa] = new KlipModel
            {
                Klipy = pliki,
                Glosy = new Dictionary<string, int>()
            };
        }

        return baza;
    }

    private void ZapiszBaze(Dictionary<string, KlipModel> baza)
    {
        var json = JsonSerializer.Serialize(baza, new JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText("baza.txt", json, Encoding.UTF8);
    }
}
