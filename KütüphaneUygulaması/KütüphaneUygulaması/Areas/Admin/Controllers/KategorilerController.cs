
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KütüphaneUygulaması.Models;

[Area("Admin")]
public class KategorilerController : Controller
{
    private readonly AppDbContext _context;

    public KategorilerController(AppDbContext context)
    {
        _context = context;
    }

    // GET: KATEGORIS
    public async Task<IActionResult> Index()    
    {
        var kategoriler = _context.Kategoriler.Include(k => k.Kitaplar);
        return View(await kategoriler.ToListAsync());
    }

    // GET: KATEGORIS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var kategori = await _context.Kategoriler
            .FirstOrDefaultAsync(m => m.Id == id);
        if (kategori == null)
        {
            return NotFound();
        }

        return View(kategori);
    }

    // GET: KATEGORIS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: KATEGORIS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,KategoriAdi")] Kategori kategori) 
    {
        
        ModelState.Remove("Kitaplar");

        if (ModelState.IsValid)
        {
            _context.Add(kategori);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(kategori);
    }

    // GET: KATEGORIS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var kategori = await _context.Kategoriler.FindAsync(id);
        if (kategori == null)
        {
            return NotFound();
        }
        return View(kategori);
    }

    // POST: KATEGORIS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,KategoriAdi")] Kategori kategori)
    {
        if (id != kategori.Id)
        {
            return NotFound();
        }

        ModelState.Remove("Kitaplar");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(kategori);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KategoriExists(kategori.Id))
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
        return View(kategori);
    }

    // GET: KATEGORIS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var kategori = await _context.Kategoriler
            .FirstOrDefaultAsync(m => m.Id == id);
        if (kategori == null)
        {
            return NotFound();
        }

        return View(kategori);
    }

    // POST: KATEGORIS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var kategori = await _context.Kategoriler.FindAsync(id);
        if (kategori != null)
        {
            _context.Kategoriler.Remove(kategori);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool KategoriExists(int? id)
    {
        return _context.Kategoriler.Any(e => e.Id == id);
    }
}
