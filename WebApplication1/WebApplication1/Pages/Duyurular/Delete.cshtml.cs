using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages.Duyurular
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Duyuru SilinecekDuyuru { get; set; }

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Sayfa açıldığında silinecek verinin detaylarını getir
        public void OnGet(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Duyurular WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            SilinecekDuyuru = new Duyuru
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Baslik = reader["Baslik"].ToString(),
                                Icerik = reader["Icerik"].ToString()
                            };
                        }
                    }
                }
            }
        }

        // Silmeyi onayla butonuna basıldığında
        public IActionResult OnPost()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Duyurular WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", SilinecekDuyuru.Id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToPage("Index");
        }
    }
}