using System.ComponentModel.DataAnnotations;

namespace OyuncuProjesi.Models
{
    public class Gamer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "isim giriniz")]
        public string Name { get; set; }

        [Required(ErrorMessage = "oyun giriniz")]
        public string Game { get; set; }

        [Required(ErrorMessage = "oynama saati giriniz")]
        public int Hour { get; set; }
    }
}
