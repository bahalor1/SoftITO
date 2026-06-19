
using KütüphaneUygulaması.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Area("Admin")]
public class YorumlarController : Controller
{
    private readonly AppDbContext _context;

    public YorumlarController(AppDbContext context)
    {
        _context = context;
    }

    // GET: YORUMS
    public async Task<IActionResult> Index()    
    {
        var yorumlar = _context.Yorumlar.Include(y => y.Kitap);
        return View(await yorumlar.ToListAsync());
    }

    // GET: YORUMS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var yorum = await _context.Yorumlar
            .FirstOrDefaultAsync(m => m.Id == id);
        if (yorum == null)
        {
            return NotFound();
        }

        return View(yorum);
    }

    // GET: YORUMS/Create
    public IActionResult Create()
    {
        ViewBag.KitapId = new SelectList(_context.Kitaplar, "Id", "KitapAdi");
        return View();
    }

    // POST: YORUMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Metin,KitapId")] Yorum yorum) 
    {
        ModelState.Remove("Kitap");

        if (ModelState.IsValid)
        {
            _context.Add(yorum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.KitapId = new SelectList(_context.Kitaplar, "Id", "KitapAdi", yorum.KitapId);
        return View(yorum);
    }

    // GET: YORUMS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var yorum = await _context.Yorumlar.FindAsync(id);
        if (yorum == null)
        {
            return NotFound();
        }
        ViewBag.KitapId = new SelectList(_context.Kitaplar, "Id", "KitapAdi", yorum.KitapId);
        return View(yorum);
    }

    // POST: YORUMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Metin,KitapId")] Yorum yorum)
    {
        if (id != yorum.Id)
        {
            return NotFound();
        }

        ModelState.Remove("Kitap");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(yorum);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YorumExists(yorum.Id))
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
        return View(yorum);
    }

    // GET: YORUMS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var yorum = await _context.Yorumlar
    .Include(y => y.Kitap)
    .FirstOrDefaultAsync(m => m.Id == id);
        if (yorum == null)
        {
            return NotFound();
        }

        return View(yorum);
    }

    // POST: YORUMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var yorum = await _context.Yorumlar.FindAsync(id);
        if (yorum != null)
        {
            _context.Yorumlar.Remove(yorum);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool YorumExists(int? id)
    {
        return _context.Yorumlar.Any(e => e.Id == id);
    }
}
