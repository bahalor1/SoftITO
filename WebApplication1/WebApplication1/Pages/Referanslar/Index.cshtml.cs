using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages.Referanslar
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<Referans> ReferansListesi { get; set; } = new List<Referans>();

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Referanslar";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ReferansListesi.Add(new Referans
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                MusteriAdi = reader["MusteriAdi"].ToString(),
                                Sirket = reader["Sirket"].ToString()
                            });
                        }
                    }
                }
            }
        }

        
    }
}