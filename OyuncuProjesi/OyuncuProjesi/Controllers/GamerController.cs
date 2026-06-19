using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OyuncuProjesi.Data;
using OyuncuProjesi.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Linq;

namespace OyuncuProjesi.Controllers
{
    public class GamerController : Controller
    {
        private readonly ApplicationDbContext context;

        public GamerController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GamerList()
        {
            var data = context.Gamers.ToList();
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult AddGamer(Gamer gamer)
        {
            var emp = new Gamer()
            {
                Name = gamer.Name,
                Game = gamer.Game,
                Hour = gamer.Hour,
            };
            context.Gamers.Add(emp);
            context.SaveChanges();
            return new JsonResult("data saved");
        }

        [HttpGet]
        public JsonResult Edit(int id)
        {
            var data = context.Gamers.Where(m => m.Id == id).SingleOrDefault();
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult Update(Gamer gamers)
        {
            context.Update(gamers);
            context.SaveChanges();
            return new JsonResult("data updated");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var data = context.Gamers.Where(m => m.Id == id).SingleOrDefault();
            if (data != null)
            {
                context.Gamers.Remove(data);
                context.SaveChanges();
                return new JsonResult("data deleted");
            }
            return new JsonResult("data not found");
        }

        public IActionResult ExportToPdf()
        {
            // QuestPDF Topluluk Lisansı
            QuestPDF.Settings.License = LicenseType.Community;

            // 1. Veri tabanından güncel oyuncu istatistikleri listesini çekin (Senin tablon: Gamers)
            var stats = context.Gamers.ToList();

            // 2. QuestPDF ile PDF dökümanını tasarlayın
            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    // Üst Bilgi (Header)
                    page.Header()
                        .Text("Oyuncu İstatistikleri Raporu")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    // İçerik (Tablo Oluşturma)
                    page.Content()
                        .PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            // Sütun genişliklerini tanımlayın
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);  // ID sütunu
                                columns.RelativeColumn();    // Oyuncu adı (esnek)
                                columns.RelativeColumn();    // Oyun adı (esnek)
                                columns.ConstantColumn(80);  // Süre sütunu
                            });

                            // Tablo Başlıkları (Header Row)
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Oyuncu Adı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Oyun Adı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Süre (Saat)").Bold();
                            });

                            // Veri Satırları (Senin Modelin: Name, Game, Hour)
                            foreach (var item in stats)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Id.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Name);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Game);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Hour.ToString());
                            }
                        });

                    // Alt Bilgi (Footer)
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Sayfa ");
                            x.CurrentPageNumber();
                        });
                });
            });

            // 3. PDF'i byte dizisine çevirip tarayıcıya indirtme
            var pdfBytes = pdfDocument.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"Oyuncu_Listesi_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public IActionResult ExportToExcel()
        {
            // EPPlus Ticari Olmayan Kullanım Lisansı
            ExcelPackage.License.SetNonCommercialPersonal("Oyuncu Projesi");

            // 1. Veri tabanından güncel listenizi çekin (Senin tablon: Gamers)
            var stats = context.Gamers.ToList();

            // 2. Bellekte (Memory) boş bir Excel dosyası oluşturun
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Oyuncu Listesi");

                // 3. Tablo Başlıklarını Yazın
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Oyuncu Adı";
                worksheet.Cells[1, 3].Value = "Oyun Adı";
                worksheet.Cells[1, 4].Value = "Süre (Saat)";

                // 4. Başlık Satırını Şıklaştırın
                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // 5. Verileri Excel Satırlarına Basın (Senin Modelin: Name, Game, Hour)
                int rowNumber = 2;
                foreach (var item in stats)
                {
                    worksheet.Cells[rowNumber, 1].Value = item.Id;
                    worksheet.Cells[rowNumber, 2].Value = item.Name;
                    worksheet.Cells[rowNumber, 3].Value = item.Game;
                    worksheet.Cells[rowNumber, 4].Value = item.Hour;
                    rowNumber++;
                }

                // 6. Sütun genişliklerini içeriğe göre otomatik ayarla
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // 7. Excel dosyasını byte dizisine çevirip tarayıcıya fırlat
                var fileBytes = package.GetAsByteArray();
                string fileName = $"Oyuncu_Listesi_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}