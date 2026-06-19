
using KütüphaneUygulaması.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Area("Admin")]
public class KitaplarController : Controller
{
    private readonly AppDbContext _context;

    public KitaplarController(AppDbContext context)
    {
        _context = context;
    }

    // GET: KITAPS
    public async Task<IActionResult> Index()
    {
        // Include komutları, sadece ID'leri değil, yazar ve kategori isimlerini de veritabanından çekip getirmemizi sağlar!
        var kitaplar = _context.Kitaplar
                               .Include(k => k.Kategori)
                               .Include(k => k.Yazar);

        return View(await kitaplar.ToListAsync());
    }

    // GET: KITAPS/Details/5
    // GET: KITAPS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Yazar, Kategori ve Yorumlar tablolarını detay sayfasına dahil ediyoruz
        var kitap = await _context.Kitaplar
            .Include(k => k.Yazar)
            .Include(k => k.Kategori)
            .Include(k => k.Yorumlar)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (kitap == null)
        {
            return NotFound();
        }

        return View(kitap);
    }

    // GET: KITAPS/Create
    public IActionResult Create()
    {
        ViewBag.KategoriId = new SelectList(_context.Kategoriler, "Id", "KategoriAdi");

        // Veritabanından Yazarları al, arkaplanda 'Id' tut, kullanıcıya 'AdSoyad' göster
        ViewBag.YazarId = new SelectList(_context.Yazarlar, "Id", "AdSoyad");

        return View();
    }

    // POST: KITAPS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,KitapAdi,YazarId,KategoriId")] Kitap kitap)
    {
   
        ModelState.Remove("Kategori");
        ModelState.Remove("Yazar");
        ModelState.Remove("Yorumlar");

        if (ModelState.IsValid)
        {
            _context.Add(kitap);
            await _context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Index)); 
        }

        ViewBag.KategoriId = new SelectList(_context.Kategoriler, "Id", "KategoriAdi", kitap.KategoriId);
        ViewBag.YazarId = new SelectList(_context.Yazarlar, "Id", "AdSoyad", kitap.YazarId);

        return View(kitap);
    }

    // GET: KITAPS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var kitap = await _context.Kitaplar.FindAsync(id);
        if (kitap == null)
        {
            return NotFound();
        }

        // EKLENEN KISIM: Açılır listeler için veritabanından isimleri çekiyoruz
        ViewBag.KategoriId = new SelectList(_context.Kategoriler, "Id", "KategoriAdi", kitap.KategoriId);
        ViewBag.YazarId = new SelectList(_context.Yazarlar, "Id", "AdSoyad", kitap.YazarId);

        return View(kitap);
    }

    // POST: KITAPS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,KitapAdi,YazarId,KategoriId")] Kitap kitap)
    {
        if (id != kitap.Id)
        {
            return NotFound();
        }

        // EKLENEN KISIM: Güvenlik duvarına takılmamak için boş listeleri ve nesneleri geçersiz saymıyoruz
        ModelState.Remove("Kategori");
        ModelState.Remove("Yazar");
        ModelState.Remove("Yorumlar");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(kitap);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KitapExists(kitap.Id))
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

        // EKLENEN KISIM: Eğer bir hata olursa form sıfırlanmasın diye açılır listeleri tekrar yüklüyoruz
        ViewBag.KategoriId = new SelectList(_context.Kategoriler, "Id", "KategoriAdi", kitap.KategoriId);
        ViewBag.YazarId = new SelectList(_context.Yazarlar, "Id", "AdSoyad", kitap.YazarId);

        return View(kitap);
    }

    // GET: KITAPS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // DÜZELTİLEN KISIM: Ekranda yazar ve kategori adı görünsün diye Include ekledik
        var kitap = await _context.Kitaplar
            .Include(k => k.Yazar)
            .Include(k => k.Kategori)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (kitap == null)
        {
            return NotFound();
        }

        return View(kitap);
    }

    // POST: KITAPS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var kitap = await _context.Kitaplar.FindAsync(id);
        if (kitap != null)
        {
            _context.Kitaplar.Remove(kitap);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool KitapExists(int? id)
    {
        return _context.Kitaplar.Any(e => e.Id == id);
    }
}
