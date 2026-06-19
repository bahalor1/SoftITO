namespace KütüphaneUygulaması.Models
{
    public class Kitap
    {
        public int Id { get; set; }
        public string KitapAdi {  get; set; }
        public int YazarId { get; set; }
        public Yazar Yazar {  get; set; }
        public int KategoriId { get; set; }
        public Kategori Kategori { get; set; }

        public ICollection<Yorum> Yorumlar { get; set; }
    }
}
