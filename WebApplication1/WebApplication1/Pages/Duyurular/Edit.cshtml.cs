using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages.Duyurular
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Duyuru GuncelDuyuru { get; set; }

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Sayfa açıldığında mevcut veriyi ID'ye göre getir
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
                            GuncelDuyuru = new Duyuru
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

        // Güncelle butonuna basıldığında (UPDATE)
        public IActionResult OnPost()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Duyurular SET Baslik = @Baslik, Icerik = @Icerik WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", GuncelDuyuru.Id);
                    cmd.Parameters.AddWithValue("@Baslik", GuncelDuyuru.Baslik);
                    cmd.Parameters.AddWithValue("@Icerik", GuncelDuyuru.Icerik);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToPage("Index");
        }
    }
}