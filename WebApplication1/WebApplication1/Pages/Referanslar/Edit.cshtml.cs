using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages.Referanslar
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Referans GuncelReferans { get; set; } = new Referans();

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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
                            GuncelReferans = new Referans
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

        public IActionResult OnPost()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Referanslar SET MusteriAdi = @MusteriAdi, Sirket = @Sirket WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", GuncelReferans.Id);
                    cmd.Parameters.AddWithValue("@MusteriAdi", GuncelReferans.MusteriAdi);
                    cmd.Parameters.AddWithValue("@Sirket", GuncelReferans.Sirket);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToPage("Index");
        }
    }
}