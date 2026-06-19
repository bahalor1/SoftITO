using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinemaUygulaması.Models;
using System.Diagnostics;


namespace SinemaUygulaması.Controllers
{
    public class HomeController(SinemaDbContext context) : Controller
    {

        private readonly SinemaDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var filmler = await _context.Filmlers.ToListAsync();
            return View(filmler);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
