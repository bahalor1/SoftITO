using KütüphaneUygulaması.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KütüphaneUygulaması.Models;

namespace KütüphaneUygulaması.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RaporlarController : Controller
    {
        private readonly AppDbContext _context;

        public RaporlarController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ReportViewModel();

            model.EnCokYorumAlanKitaplar = await _context.Kitaplar
    .Include(k => k.Yazar)
    .Include(k => k.Kategori)
    .Include(k => k.Yorumlar)
    .Select(k => new KitapRaporItem
    {
        KitapId = k.Id,
        KitapAdi = k.KitapAdi,
        YazarAdi = k.Yazar.AdSoyad,
        KategoriAdi = k.Kategori.KategoriAdi,
        YorumSayisi = k.Yorumlar.Count  // ← BU EKSİK, EKLE
    })
    .OrderByDescending(k => k.YorumSayisi)  // ← En çok yorumu üste sırala
    .Take(10)  // ← Top 10
    .ToListAsync();

            model.KategoriyeGoreKitapSayisi = await _context.Kategoriler
                .Select(k => new KategoriKitapSayisiItem
                {
                    KategoriId = k.Id,
                    KategoriAdi = k.KategoriAdi,
                    KitapSayisi = k.Kitaplar.Count
                })
                .OrderByDescending(x => x.KitapSayisi)
                .ToListAsync();

            model.EnCokKitabiOlanYazarlar = await _context.Yazarlar
                .Select(y => new YazarRaporItem
                {
                    YazarId = y.Id,
                    YazarAdi = y.AdSoyad,
                    KitapSayisi = y.Kitaplar.Count
                })
                .OrderByDescending(x => x.KitapSayisi)
                .Take(10)
                .ToListAsync();

            return View(model);
        }
    }
}