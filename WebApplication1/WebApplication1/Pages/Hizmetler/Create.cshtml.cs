using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages.Hizmetler
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Hizmet YeniHizmet { get; set; } = new Hizmet();

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Hizmetler (HizmetAdi, KisaAciklama) VALUES (@HizmetAdi, @KisaAciklama)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@HizmetAdi", YeniHizmet.HizmetAdi);
                    cmd.Parameters.AddWithValue("@KisaAciklama", YeniHizmet.KisaAciklama);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToPage("Index");
        }
    }
}