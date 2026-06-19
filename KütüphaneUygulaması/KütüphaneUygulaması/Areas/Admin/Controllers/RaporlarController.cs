using KütüphaneUygulaması.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KütüphaneUygulaması.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Linq;

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

        public IActionResult ExportToPdf()
        {
            // QuestPDF Lisansı
            QuestPDF.Settings.License = LicenseType.Community;

            // HATA DÜZELTİLDİ: İlişkili tabloları (Yazar, Kategori, Yorumlar) dahil ettik
            var veriListesi = _context.Kitaplar
                .Include(k => k.Yazar)
                .Include(k => k.Kategori)
                .Include(k => k.Yorumlar)
                .OrderByDescending(k => k.Yorumlar.Count) // En çok yorum alana göre sırala
                .ToList();

            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial")); // Yazı tipini biraz küçülttük sığması için

                    // Rapor Başlığı
                    page.Header()
                        .Text("Kütüphane Kitap Raporu")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    // Tablo Yapısı
                    page.Content()
                        .PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            // 5 Sütun olarak ayarladık
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);  // ID
                                columns.RelativeColumn(2);   // Kitap Adı (Daha geniş)
                                columns.RelativeColumn(2);   // Yazar
                                columns.RelativeColumn(2);   // Kategori
                                columns.ConstantColumn(80);  // Yorum Sayısı
                            });

                            // Tablo Başlık İsimleri
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Kitap Adı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Yazar").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Kategori").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Yorum Sayısı").Bold();
                            });

                            // HATA DÜZELTİLDİ: Sınıfları değil, içindeki özellikleri (.KitapAdi, .AdSoyad, .Count) çağırdık
                            foreach (var item in veriListesi)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Id.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.KitapAdi ?? "-");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Yazar?.AdSoyad ?? "-");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Kategori?.KategoriAdi ?? "-");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Yorumlar?.Count.ToString() ?? "0");
                            }
                        });

                    page.Footer().AlignCenter().Text(x => { x.Span("Sayfa "); x.CurrentPageNumber(); });
                });
            });

            var pdfBytes = pdfDocument.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"Kitap_Raporu_{DateTime.Now:yyyyMMdd}.pdf");
        }

        // ==========================================
        // EXCEL INDIRME METODU
        // ==========================================
        public IActionResult ExportToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Kutuphane Uygulamasi");

            // HATA DÜZELTİLDİ: İlişkili tablolar (Include) eklendi
            var veriListesi = _context.Kitaplar
                .Include(k => k.Yazar)
                .Include(k => k.Kategori)
                .Include(k => k.Yorumlar)
                .OrderByDescending(k => k.Yorumlar.Count)
                .ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Kitap Raporu");

                // Excel Başlık Satırı
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Kitap Adı";
                worksheet.Cells[1, 3].Value = "Yazar";
                worksheet.Cells[1, 4].Value = "Kategori";
                worksheet.Cells[1, 5].Value = "Yorum Sayısı";

                // Başlık Tasarımı (1'den 5. sütuna kadar)
                using (var range = worksheet.Cells[1, 1, 1, 5])
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
                    // HATA DÜZELTİLDİ: Doğru property'ler (özellikler) atandı
                    worksheet.Cells[rowNumber, 1].Value = item.Id;
                    worksheet.Cells[rowNumber, 2].Value = item.KitapAdi;
                    worksheet.Cells[rowNumber, 3].Value = item.Yazar?.AdSoyad;
                    worksheet.Cells[rowNumber, 4].Value = item.Kategori?.KategoriAdi;
                    worksheet.Cells[rowNumber, 5].Value = item.Yorumlar?.Count ?? 0;
                    rowNumber++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var fileBytes = package.GetAsByteArray();
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Kitap_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
            }
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