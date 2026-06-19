
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinemaUygulaması.Models;

public class FilmlersController : Controller
{
    private readonly SinemaDbContext _context;

    public FilmlersController(SinemaDbContext context)
    {
        _context = context;
    }

    // GET: FILMLERS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Filmlers.ToListAsync());
    }

    // GET: FILMLERS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var filmler = await _context.Filmlers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (filmler == null)
        {
            return NotFound();
        }

        return View(filmler);
    }

    // GET: FILMLERS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: FILMLERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FilmAdi,Tur,SureDakika,Seanslars")] Filmler filmler)
    {
        if (ModelState.IsValid)
        {
            _context.Add(filmler);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(filmler);
    }

    // GET: FILMLERS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var filmler = await _context.Filmlers.FindAsync(id);
        if (filmler == null)
        {
            return NotFound();
        }
        return View(filmler);
    }

    // POST: FILMLERS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,FilmAdi,Tur,SureDakika,Seanslars")] Filmler filmler)
    {
        if (id != filmler.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(filmler);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmlerExists(filmler.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(filmler);
    }

    // GET: FILMLERS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var filmler = await _context.Filmlers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (filmler == null)
        {
            return NotFound();
        }

        return View(filmler);
    }

    // POST: FILMLERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var filmler = await _context.Filmlers.FindAsync(id);
        if (filmler != null)
        {
            _context.Filmlers.Remove(filmler);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool FilmlerExists(int? id)
    {
        return _context.Filmlers.Any(e => e.Id == id);
    }
}
