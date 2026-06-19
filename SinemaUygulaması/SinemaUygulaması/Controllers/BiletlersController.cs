using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SinemaUygulaması.Models;

public class BiletlersController : Controller
{
    private readonly SinemaDbContext _context;

    public BiletlersController(SinemaDbContext context)
    {
        _context = context;
    }

    // GET: BILETLERS
    public async Task<IActionResult> Index()
    {
        var sinemaDbContext = _context.Biletlers
            .Include(b => b.Seans)
            .ThenInclude(s => s.Film);
        return View(await sinemaDbContext.ToListAsync());
    }

    // GET: BILETLERS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        // HATA DÜZELTİLDİ: Include eklendi
        var biletler = await _context.Biletlers
            .Include(b => b.Seans)
            .ThenInclude(s => s.Film)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (biletler == null) return NotFound();

        return View(biletler);
    }

    // GET: BILETLERS/Create
    public IActionResult Create()
    {
        var seansListesi = _context.Seanslars
            .Include(s => s.Film)
            .Select(s => new { Id = s.Id, GosterimBilgisi = s.Film.FilmAdi + " (" + s.SalonAdi + ")" }).ToList();

        ViewData["SeansId"] = new SelectList(seansListesi, "Id", "GosterimBilgisi");
        return View();
    }

    // POST: BILETLERS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,SeansId,MusteriAdSoyad,KoltukNo")] Biletler biletler) // HATA DÜZELTİLDİ: Bind içindeki "Seans" silindi
    {
        if (ModelState.IsValid)
        {
            _context.Add(biletler);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Hata olursa dropdown'ı tekrar doldur
        var seansListesi = _context.Seanslars.Include(s => s.Film).Select(s => new { Id = s.Id, GosterimBilgisi = s.Film.FilmAdi + " (" + s.SalonAdi + ")" }).ToList();
        ViewData["SeansId"] = new SelectList(seansListesi, "Id", "GosterimBilgisi", biletler.SeansId);
        return View(biletler);
    }

    // GET: BILETLERS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var biletler = await _context.Biletlers.FindAsync(id);
        if (biletler == null) return NotFound();

        // HATA DÜZELTİLDİ: Edit ekranına da Film+Salon açılır listesi gönderildi
        var seansListesi = _context.Seanslars.Include(s => s.Film).Select(s => new { Id = s.Id, GosterimBilgisi = s.Film.FilmAdi + " (" + s.SalonAdi + ")" }).ToList();
        ViewData["SeansId"] = new SelectList(seansListesi, "Id", "GosterimBilgisi", biletler.SeansId);
        return View(biletler);
    }

    // POST: BILETLERS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,SeansId,MusteriAdSoyad,KoltukNo")] Biletler biletler)
    {
        if (id != biletler.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(biletler);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BiletlerExists(biletler.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }

        var seansListesi = _context.Seanslars.Include(s => s.Film).Select(s => new { Id = s.Id, GosterimBilgisi = s.Film.FilmAdi + " (" + s.SalonAdi + ")" }).ToList();
        ViewData["SeansId"] = new SelectList(seansListesi, "Id", "GosterimBilgisi", biletler.SeansId);
        return View(biletler);
    }

    // GET: BILETLERS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        // HATA DÜZELTİLDİ: Silme ekranında film adını görebilmek için Include eklendi
        var biletler = await _context.Biletlers
            .Include(b => b.Seans)
            .ThenInclude(s => s.Film)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (biletler == null) return NotFound();

        return View(biletler);
    }

    // POST: BILETLERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var biletler = await _context.Biletlers.FindAsync(id);
        if (biletler != null)
        {
            _context.Biletlers.Remove(biletler);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BiletlerExists(int? id)
    {
        return _context.Biletlers.Any(e => e.Id == id);
    }
}