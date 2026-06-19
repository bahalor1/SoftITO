
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SinemaUygulaması.Models;

public class SeanslarsController : Controller
{
    private readonly SinemaDbContext _context;

    public SeanslarsController(SinemaDbContext context)
    {
        _context = context;
    }

    // GET: SEANSLARS
    public async Task<IActionResult> Index()
    {
        var sinemaDbContext = _context.Seanslars.Include(s => s.Film);
        return View(model: await sinemaDbContext.ToListAsync());
    }

    // GET: SEANSLARS/Details/5
    public async Task<IActionResult> Details(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    // Araya Include komutunu ekledik ki detaylarına baktığımız seansın Filmi de gelsin!
    var seanslar = await _context.Seanslars
        .Include(s => s.Film)
        .FirstOrDefaultAsync(m => m.Id == id);
        
    if (seanslar == null)
    {
        return NotFound();
    }

    return View(seanslar);
}

    // GET: SEANSLARS/Create
    public IActionResult Create()
    {
        // Film isimlerini dropdown (açılır liste) için sayfaya gönderiyoruz
        ViewData["FilmId"] = new SelectList(_context.Filmlers, "Id", "FilmAdi");
        return View();
    }

    // POST: SEANSLARS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FilmId,SalonAdi,BaslangicSaati,Biletlers,Film")] Seanslar seanslar)
    {
        if (ModelState.IsValid)
        {
            _context.Add(seanslar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(seanslar);
    }

    // GET: SEANSLARS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var seans = await _context.Seanslars.FindAsync(id);
        if (seans == null) return NotFound();

        // Filmleri dropdown (açılır liste) için sayfaya gönderiyoruz
        ViewData["FilmId"] = new SelectList(_context.Filmlers, "Id", "FilmAdi", seans.FilmId);
        return View(seans);
    }

    // POST: SEANSLARS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,FilmId,SalonAdi,BaslangicSaati,Biletlers,Film")] Seanslar seanslar)
    {
        if (id != seanslar.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(seanslar);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeanslarExists(seanslar.Id))
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
        return View(seanslar);
    }

    // GET: SEANSLARS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var seanslar = await _context.Seanslars
            .FirstOrDefaultAsync(m => m.Id == id);
        if (seanslar == null)
        {
            return NotFound();
        }

        return View(seanslar);
    }

    // POST: SEANSLARS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var seanslar = await _context.Seanslars.FindAsync(id);
        if (seanslar != null)
        {
            _context.Seanslars.Remove(seanslar);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SeanslarExists(int? id)
    {
        return _context.Seanslars.Any(e => e.Id == id);
    }
}
