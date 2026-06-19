using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SinemaUygulaması.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Linq;

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

        public IActionResult ExportToPdf()
        {
            // QuestPDF Lisansı
            QuestPDF.Settings.License = LicenseType.Community;

            // HATA DÜZELTİLDİ: Artık senin projendeki Biletler tablosu çekiliyor
            var veriListesi = _context.Biletlers.ToList();

            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    // Rapor Başlığı
                    page.Header()
                        .Text("Bilet Satış Raporu") // Başlık güncellendi
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    // Tablo Yapısı
                    page.Content()
                        .PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            // 2 Sütunluk yapı oluşturduk
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);  // ID Sütunu
                                columns.RelativeColumn();    // Müşteri Adı Sütunu
                            });

                            // Tablo Başlık İsimleri
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Bilet ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Müşteri Ad Soyad").Bold();
                            });

                            // HATA DÜZELTİLDİ: Senin Modelindeki Sütunlar (Id ve MusteriAdSoyad) eklendi
                            foreach (var item in veriListesi)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Id.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.MusteriAdSoyad);
                            }
                        });

                    page.Footer().AlignCenter().Text(x => { x.Span("Sayfa "); x.CurrentPageNumber(); });
                });
            });

            var pdfBytes = pdfDocument.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"Bilet_Raporu_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public IActionResult ExportToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Sinema Uygulamasi");

            // HATA DÜZELTİLDİ: Senin projendeki Biletler tablosu
            var veriListesi = _context.Biletlers.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Bilet Listesi");

                // Başlıklar Güncellendi
                worksheet.Cells[1, 1].Value = "Bilet ID";
                worksheet.Cells[1, 2].Value = "Müşteri Ad Soyad";

                // Stil kısmı 2 sütuna göre ayarlandı
                using (var range = worksheet.Cells[1, 1, 1, 2])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                int rowNumber = 2;
                foreach (var item in veriListesi)
                {
                    // HATA DÜZELTİLDİ: Senin modelinden gelen veriler eklendi
                    worksheet.Cells[rowNumber, 1].Value = item.Id;
                    worksheet.Cells[rowNumber, 2].Value = item.MusteriAdSoyad;
                    rowNumber++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var fileBytes = package.GetAsByteArray();
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Bilet_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
            }
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