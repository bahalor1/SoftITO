namespace KütüphaneUygulaması.Models
{
    public class Yazar
    {
        public int Id { get; set; }
        public string AdSoyad {  get; set; }
        public ICollection<Kitap> Kitaplar { get; set; }
    }
}
