using Microsoft.AspNetCore.Mvc;
using OyuncuProjesi.Data;
using OyuncuProjesi.Models;

namespace OyuncuProjesi.Controllers
{
    public class GamerController : Controller
    {
        private readonly ApplicationDbContext context;

        public GamerController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GamerList()
        {
            var data = context.Gamers.ToList();
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult AddGamer(Gamer gamer)
        {
            var emp = new Gamer()
            {
                Name = gamer.Name,
                Game = gamer.Game,
                Hour = gamer.Hour,
            };
            context.Gamers.Add(emp);
            context.SaveChanges();
            return new JsonResult("data saved");

        }

        public JsonResult Edit(int id)
        {
            var data = context.Gamers.Where(m => m.Id == id).SingleOrDefault();
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult Update(Gamer gamers)
        {
            context.Update(gamers);
            context.SaveChanges();
            return new JsonResult("data updated");
        }

        public JsonResult Delete(int id)
        {
            var data = context.Gamers.Where(m => m.Id == id).SingleOrDefault();
            context.Gamers.Remove(data);
            context.SaveChanges();
            return new JsonResult("data deleted");
        }
    }
}
