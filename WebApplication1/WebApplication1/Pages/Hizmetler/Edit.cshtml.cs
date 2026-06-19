using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages.Hizmetler
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Hizmet GuncelHizmet { get; set; } = new Hizmet();

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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
                            GuncelHizmet = new Hizmet
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

        public IActionResult OnPost()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Hizmetler SET HizmetAdi = @HizmetAdi, KisaAciklama = @KisaAciklama WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", GuncelHizmet.Id);
                    cmd.Parameters.AddWithValue("@HizmetAdi", GuncelHizmet.HizmetAdi);
                    cmd.Parameters.AddWithValue("@KisaAciklama", GuncelHizmet.KisaAciklama);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToPage("Index");
        }
    }
}