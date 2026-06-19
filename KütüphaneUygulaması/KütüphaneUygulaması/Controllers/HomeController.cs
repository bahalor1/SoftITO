using KütüphaneUygulaması.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KütüphaneUygulaması.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context) { _context = context; }

        public async Task<IActionResult> Index(string aramaKelimesi,string aramaTuru)
        {
            var vitrinKitaplari = _context.Kitaplar
                                .Include(k => k.Kategori)
                                .Include(k => k.Yazar)
                                .Include(k => k.Yorumlar)
                                .AsQueryable();

            // Eğer ziyaretçi arama kutusuna bir şey yazıp gönderdiyse filtrele
            if (!string.IsNullOrEmpty(aramaKelimesi))
            {
                switch (aramaTuru)
                {
                    case "Yazar":
                        vitrinKitaplari = vitrinKitaplari.Where(k => k.Yazar.AdSoyad.Contains(aramaKelimesi));
                        break;
                    case "Kategori":
                        vitrinKitaplari = vitrinKitaplari.Where(k => k.Kategori.KategoriAdi.Contains(aramaKelimesi));
                        break;
                    case "KitapAdi":
                        vitrinKitaplari = vitrinKitaplari.Where(k => k.KitapAdi.Contains(aramaKelimesi));
                        break;
                }
            }

            return View(await vitrinKitaplari.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
