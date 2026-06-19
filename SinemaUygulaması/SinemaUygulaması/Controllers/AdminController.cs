using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SinemaUygulaması.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinemaUygulaması.Controllers
{
    public class RaporVerisi
    {
        public required string Etiket { get; set; }
        public int Deger { get; set; }    
    }

    public class AdminController : Controller
    {
        private readonly SinemaDbContext _context;

        public AdminController(SinemaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Raporlar(int? raporId, DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            int secilenRapor = raporId ?? 1;
            ViewBag.SecilenRapor = secilenRapor;

            var basTarih = baslangicTarihi ?? DateTime.Now.AddMonths(-1);
            var bitTarih = bitisTarihi ?? DateTime.Now.AddMonths(1);

            ViewBag.Baslangic = basTarih.ToString("yyyy-MM-dd");
            ViewBag.Bitis = bitTarih.ToString("yyyy-MM-dd");

            var raporSonuclari = new List<RaporVerisi>();


            switch (secilenRapor)
            {
                case 1: 
                    raporSonuclari = await _context.Biletlers
                        .GroupBy(b => b.Seans.Film.FilmAdi)
                        .Select(g => new RaporVerisi { Etiket = g.Key, Deger = g.Count() })
                        .OrderByDescending(r => r.Deger)
                        .ToListAsync();
                    ViewBag.RaporBaslik = "Film Bazlı Toplam Bilet Satış Raporu";
                    ViewBag.SutunAdi = "Film Adı";
                    break;

                case 2: 
                    raporSonuclari = await _context.Biletlers
                        .GroupBy(b => b.Seans.SalonAdi)
                        .Select(g => new RaporVerisi { Etiket = g.Key, Deger = g.Count() })
                        .OrderByDescending(r => r.Deger)
                        .ToListAsync();
                    ViewBag.RaporBaslik = "Salon Bazlı Doluluk ve Yoğunluk Raporu";
                    ViewBag.SutunAdi = "Salon İsmi";
                    break;

                case 3: 
                    raporSonuclari = await _context.Biletlers
                        .GroupBy(b => b.MusteriAdSoyad)
                        .Select(g => new RaporVerisi { Etiket = g.Key, Deger = g.Count() })
                        .OrderByDescending(r => r.Deger)
                        .Take(5)
                        .ToListAsync();
                    ViewBag.RaporBaslik = "En Çok Bilet Alan İlk 5 Müşteri";
                    ViewBag.SutunAdi = "Müşteri Ad Soyad";
                    break;

                case 4:
                    var saatlikGruplar = await _context.Biletlers.GroupBy(b => new { b.Seans.BaslangicSaati.Hour, b.Seans.BaslangicSaati.Minute }).Select(g => new
                     {
                        Saat = g.Key.Hour,
                        Dakika = g.Key.Minute,
                        Toplam = g.Count()
                     }).OrderByDescending(r => r.Toplam).ToListAsync();

                    raporSonuclari = saatlikGruplar.Select(x => new RaporVerisi
                    {
                        Etiket = $"{x.Saat:00}:{x.Dakika:00} Seansları",
                        Deger = x.Toplam
                    }).ToList();

                    ViewBag.RaporBaslik = "Saat Dilimlerine Göre Yoğunluk Raporu";
                    ViewBag.SutunAdi = "Seans Başlangıç Saati";
                    break;

                case 5: 
                    raporSonuclari = await _context.Biletlers
                        .Where(b => b.Seans.BaslangicSaati >= basTarih && b.Seans.BaslangicSaati <= bitTarih)
                        .GroupBy(b => b.Seans.Film.FilmAdi)
                        .Select(g => new RaporVerisi { Etiket = g.Key, Deger = g.Count() })
                        .OrderByDescending(r => r.Deger)
                        .ToListAsync();
                    ViewBag.RaporBaslik = $"{basTarih.ToString("dd.MM.yyyy")} - {bitTarih.ToString("dd.MM.yyyy")} Tarihleri Arasındaki Satışlar";
                    ViewBag.SutunAdi = "Film Adı (Filtreli Dönem)";
                    break;
            }

            return View(raporSonuclari);
        }
    }
}