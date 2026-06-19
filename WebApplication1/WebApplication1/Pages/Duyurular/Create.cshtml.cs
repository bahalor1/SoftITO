using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages.Duyurular
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        // Formdan gelecek veriyi bu modele bağla (Bind)
        [BindProperty]
        public Duyuru YeniDuyuru { get; set; }

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            // Sayfa ilk açıldığında yapılacak bir şey yok
        }

        // Form gönderildiğinde (Kaydet) çalışacak metod (CREATE)
        public IActionResult OnPost()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Duyurular (Baslik, Icerik) VALUES (@Baslik, @Icerik)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Baslik", YeniDuyuru.Baslik);
                    cmd.Parameters.AddWithValue("@Icerik", YeniDuyuru.Icerik);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToPage("Index"); // İşlem bitince listeye dön
        }
    }
}