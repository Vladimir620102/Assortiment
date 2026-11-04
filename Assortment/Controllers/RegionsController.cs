
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assortment.Domain.Entities;
using Assortment.Data;

public class RegionsController : Controller
{
    private readonly AppDbContext _db;
    public RegionsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(bool includeInactive = false, CancellationToken ct = default)
    {
        var q = _db.Regions.AsNoTracking().AsQueryable();
        if (!includeInactive) q = q.Where(r => r.IsActive);
        ViewBag.IncludeInactive = includeInactive;
        return View(await q.OrderBy(r => r.Name).ToListAsync(ct));
    }

    public IActionResult Create() => View(new Region());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Region model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return View(model);
        if (await _db.Regions.AnyAsync(r => r.Name == model.Name, ct))
        {
            ModelState.AddModelError(nameof(model.Name), $"Регион «{model.Name}» уже существует");
            return View(model);
        }
        model.Id = Guid.NewGuid();
        model.CreatedAt = DateTimeOffset.UtcNow;
        _db.Regions.Add(model);
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Регион «{model.Name}» создан";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct = default)
    {
        var r = await _db.Regions.FindAsync(new object[] { id }, ct);
        return r is null ? NotFound() : View(r);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Region model, CancellationToken ct = default)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        if (await _db.Regions.AnyAsync(r => r.Name == model.Name && r.Id != id, ct))
        {
            ModelState.AddModelError(nameof(model.Name), $"Регион «{model.Name}» уже существует");
            return View(model);
        }
        var e = await _db.Regions.FindAsync(new object[] { id }, ct);
        if (e is null) return NotFound();
        e.Name = model.Name; e.Translate = model.Translate; e.ExternalCode = model.ExternalCode;
        e.IsActive = model.IsActive; e.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Регион «{e.Name}» обновлён";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct = default)
    {
        var r = await _db.Regions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        return r is null ? NotFound() : View(r);
    }

    [HttpPost, ActionName("Deactivate"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateConfirmed(Guid id, CancellationToken ct = default)
    {
        var r = await _db.Regions.FindAsync(new object[] { id }, ct);
        if (r is null) return NotFound();
        if (await _db.Countries.AnyAsync(c => c.RegionId == id, ct))
        {
            TempData["Success"] = null;
            TempData["Error"] = $"Нельзя деактивировать регион: в нём есть страны";
            return RedirectToAction(nameof(Index));
        }
        r.IsActive = false; r.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Регион «{r.Name}» деактивирован";
        return RedirectToAction(nameof(Index));
    }




}
