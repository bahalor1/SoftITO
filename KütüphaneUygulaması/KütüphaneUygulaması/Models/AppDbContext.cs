using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace KütüphaneUygulaması.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Kategori> Kategoriler {  get; set; }
        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<Yorum> Yorumlar {  get; set; }
        public DbSet<Yazar> Yazarlar {  get; set;}
    }
}
