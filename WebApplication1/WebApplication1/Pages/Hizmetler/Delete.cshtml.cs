using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages.Hizmetler
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Hizmet SilinecekHizmet { get; set; } = new Hizmet();

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Sayfa açıldığında silinecek hizmetin detaylarını getirir
        public void OnGet(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Hizmetler WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            SilinecekHizmet = new Hizmet
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                HizmetAdi = reader["HizmetAdi"].ToString(),
                                KisaAciklama = reader["KisaAciklama"].ToString()
                            };
                        }
                    }
                }
            }
        }

        // "Evet, Sil" butonuna basıldığında çalışır
        public IActionResult OnPost()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Hizmetler WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", SilinecekHizmet.Id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToPage("Index");
        }
    }
}