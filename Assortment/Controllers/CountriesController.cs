
using Assortment.Data;
using Assortment.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class CountriesController : Controller
{
    private readonly AppDbContext _db;
    public CountriesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(bool includeInactive = false, Guid? regionId = null, CancellationToken ct = default)
    {
        var q = _db.Countries.Include(c => c.Region).AsNoTracking().AsQueryable();
        if (!includeInactive) q = q.Where(c => c.IsActive);
        if (regionId is not null) q = q.Where(c => c.RegionId == regionId);
        ViewBag.IncludeInactive = includeInactive;
        ViewBag.RegionId = regionId;
        ViewBag.Regions = new SelectList(await _db.Regions.OrderBy(r => r.Name).ToListAsync(ct), "Id", "Name", regionId);
        return View(await q.OrderBy(c => c.Name).ToListAsync(ct));
    }

    public async Task<IActionResult> Create(CancellationToken ct = default)
    {
        await PopulateRegionsAsync(ct);
        return View(new Country());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Country model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) { await PopulateRegionsAsync(ct); return View(model); }
        if (await _db.Countries.AnyAsync(c => c.IsoCode == model.IsoCode, ct))
        {
            ModelState.AddModelError(nameof(model.IsoCode), "Страна с таким ISO уже существует");
            await PopulateRegionsAsync(ct); return View(model);
        }
        model.Id = Guid.NewGuid(); model.CreatedAt = DateTimeOffset.UtcNow;
        _db.Countries.Add(model);
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Страна «{model.Name}» создана";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct = default)
    {
        var c = await _db.Countries.FindAsync(new object[] { id }, ct);
        if (c is null) return NotFound();
        await PopulateRegionsAsync(ct, c.RegionId);
        return View(c);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Country model, CancellationToken ct = default)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) { await PopulateRegionsAsync(ct, model.RegionId); return View(model); }
        if (await _db.Countries.AnyAsync(c => c.IsoCode == model.IsoCode && c.Id != id, ct))
        {
            ModelState.AddModelError(nameof(model.IsoCode), "Страна с таким ISO уже существует");
            await PopulateRegionsAsync(ct, model.RegionId); return View(model);
        }
        var e = await _db.Countries.FindAsync(new object[] { id }, ct);
        if (e is null) return NotFound();
        e.RegionId = model.RegionId; e.Name = model.Name; e.IsoCode = model.IsoCode;
        e.Translate = model.Translate; e.IsActive = model.IsActive; e.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Страна «{e.Name}» обновлена";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct = default)
    {
        var c = await _db.Countries.Include(x => x.Region).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        return c is null ? NotFound() : View(c);
    }

    [HttpPost, ActionName("Deactivate"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateConfirmed(Guid id, CancellationToken ct = default)
    {
        var c = await _db.Countries.FindAsync(new object[] { id }, ct);
        if (c is null) return NotFound();
        if (await _db.States.AnyAsync(s => s.CountryId == id, ct))
        {
            TempData["Error"] = "Нельзя деактивировать страну: в ней есть провинции";
            return RedirectToAction(nameof(Index));
        }
        c.IsActive = false; c.UpdatedAt = DateTimeOffset.UtcNow; 
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Страна «{c.Name}» деактивирована";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateRegionsAsync(CancellationToken ct, Guid? selected = null)
    {
        ViewBag.Regions = new SelectList(
            await _db.Regions.Where(r => r.IsActive).OrderBy(r => r.Name).ToListAsync(ct),
            "Id", "Name", selected);
    }
}
