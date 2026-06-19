namespace KütüphaneUygulaması.Models
{
    public class Yorum
    {
        public int Id { get; set; }
        public string Metin {  get; set; }
        public int KitapId { get; set; }
        public Kitap Kitap { get; set; } 
    }
}
