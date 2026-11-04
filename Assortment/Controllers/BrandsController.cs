
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assortment.Domain.Entities;
using Assortment.Data;

public class BrandsController : Controller
{

    private readonly AppDbContext _db;
    public BrandsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(bool includeInactive = false, CancellationToken ct = default)
    {
        var q = _db.Brands.AsNoTracking().AsQueryable();
        if (!includeInactive) q = q.Where(b => b.IsActive);
        ViewBag.IncludeInactive = includeInactive;
        return View(await q.OrderBy(b => b.Name).ToListAsync(ct));
    }

    public IActionResult Create() => View(new Brand());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Brand model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return View(model);
        if (await _db.Brands.AnyAsync(b => b.Name == model.Name, ct))
        {
            ModelState.AddModelError(nameof(model.Name), "Бренд с таким названием уже существует");
            return View(model);
        }
        model.Id = Guid.NewGuid(); model.CreatedAt = DateTimeOffset.UtcNow;
        _db.Brands.Add(model); await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Бренд «{model.Name}» создан";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct = default)
    {
        var b = await _db.Brands.FindAsync(new object[] { id }, ct);
        return b is null ? NotFound() : View(b);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Brand model, CancellationToken ct = default)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        if (await _db.Brands.AnyAsync(b => b.Name == model.Name && b.Id != id, ct))
        {
            ModelState.AddModelError(nameof(model.Name), "Бренд с таким названием уже существует");
            return View(model);
        }
        var e = await _db.Brands.FindAsync(new object[] { id }, ct);
        if (e is null) return NotFound();
        e.Name = model.Name; e.Code = model.Code; e.Translate = model.Translate;
        e.IsActive = model.IsActive; e.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Бренд «{e.Name}» обновлён";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct = default)
    {
        var b = await _db.Brands.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        return b is null ? NotFound() : View(b);
    }

    [HttpPost, ActionName("Deactivate"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateConfirmed(Guid id, CancellationToken ct = default)
    {
        var b = await _db.Brands.FindAsync(new object[] { id }, ct);
        if (b is null) return NotFound();
        b.IsActive = false; b.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Бренд «{b.Name}» деактивирован";
        return RedirectToAction(nameof(Index));
    }
}
