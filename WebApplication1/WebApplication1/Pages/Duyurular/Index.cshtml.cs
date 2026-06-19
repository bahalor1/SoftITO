using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models; 

namespace WebApplication1.Pages.Duyurular
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<Duyuru> DuyuruListesi { get; set; } = new List<Duyuru>();

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void OnGet()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Duyurular";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DuyuruListesi.Add(new Duyuru
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Baslik = reader["Baslik"].ToString(),
                                Icerik = reader["Icerik"].ToString()
                            });
                        }
                    }
                }
            }
        }
    }
}