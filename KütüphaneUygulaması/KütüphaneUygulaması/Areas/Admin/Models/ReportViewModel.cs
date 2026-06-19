using Microsoft.AspNetCore.Mvc;

namespace KütüphaneUygulaması.Areas.Admin.Models
{
    public class ReportViewModel
    {
        public List<KitapRaporItem> EnCokYorumAlanKitaplar { get; set; } = new();
        public List<KategoriKitapSayisiItem> KategoriyeGoreKitapSayisi { get; set; } = new();
        public List<YazarRaporItem> EnCokKitabiOlanYazarlar { get; set; } = new();
    }

    public class KitapRaporItem
    {
        public int KitapId { get; set; }
        public string KitapAdi { get; set; } = string.Empty;
        public string YazarAdi { get; set; } = string.Empty;
        public string KategoriAdi { get; set; } = string.Empty;
        public int YorumSayisi { get; set; }
    }

    public class KategoriKitapSayisiItem
    {
        public int KategoriId { get; set; }
        public string KategoriAdi { get; set; } = string.Empty;
        public int KitapSayisi { get; set; }
    }

    public class YazarRaporItem
    {
        public int YazarId { get; set; }
        public string YazarAdi { get; set; } = string.Empty;
        public int KitapSayisi { get; set; }
    }
}
