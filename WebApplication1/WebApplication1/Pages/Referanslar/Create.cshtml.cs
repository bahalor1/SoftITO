using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages.Referanslar
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Referans YeniReferans { get; set; } = new Referans();

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
                string query = "INSERT INTO Referanslar (MusteriAdi, Sirket) VALUES (@MusteriAdi, @Sirket)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MusteriAdi", YeniReferans.MusteriAdi);
                    cmd.Parameters.AddWithValue("@Sirket", YeniReferans.Sirket);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToPage("Index");
        }
    }
}