
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KütüphaneUygulaması.Models;

[Area("Admin")]
public class YazarlarController : Controller
{
    private readonly AppDbContext _context;

    public YazarlarController(AppDbContext context)
    {
        _context = context;
    }

    // GET: YAZARS
    public async Task<IActionResult> Index()
    {
        // Yazarın kitaplarını listesiyle beraber getiriyoruz
        var yazarlar = _context.Yazarlar.Include(y => y.Kitaplar);
        return View(await yazarlar.ToListAsync());
    }

    // GET: YAZARS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var yazar = await _context.Yazarlar
            .FirstOrDefaultAsync(m => m.Id == id);
        if (yazar == null)
        {
            return NotFound();
        }

        return View(yazar);
    }

    // GET: YAZARS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: YAZARS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,AdSoyad")] Yazar yazar)
    {
        ModelState.Remove("Kitaplar");

        if (ModelState.IsValid)
        {
            _context.Add(yazar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(yazar);
    }

    // GET: YAZARS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var yazar = await _context.Yazarlar.FindAsync(id);
        if (yazar == null)
        {
            return NotFound();
        }
        return View(yazar);
    }

    // POST: YAZARS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,AdSoyad")] Yazar yazar)
    {
        if (id != yazar.Id)
        {
            return NotFound();
        }
        ModelState.Remove("Kitaplar");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(yazar);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YazarExists(yazar.Id))
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
        return View(yazar);
    }

    // GET: YAZARS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var yazar = await _context.Yazarlar
            .FirstOrDefaultAsync(m => m.Id == id);
        if (yazar == null)
        {
            return NotFound();
        }

        return View(yazar);
    }

    // POST: YAZARS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var yazar = await _context.Yazarlar.FindAsync(id);
        if (yazar != null)
        {
            _context.Yazarlar.Remove(yazar);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool YazarExists(int? id)
    {
        return _context.Yazarlar.Any(e => e.Id == id);
    }
}
