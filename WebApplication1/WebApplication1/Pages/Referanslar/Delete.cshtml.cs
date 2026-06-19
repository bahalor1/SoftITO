using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages.Referanslar
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Referans SilinecekReferans { get; set; } = new Referans();

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Sayfa açıldığında silinecek referansın detaylarını getirir
        public void OnGet(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Referanslar WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            SilinecekReferans = new Referans
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                MusteriAdi = reader["MusteriAdi"].ToString(),
                                Sirket = reader["Sirket"].ToString()
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
                string query = "DELETE FROM Referanslar WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", SilinecekReferans.Id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToPage("Index");
        }
    }
}