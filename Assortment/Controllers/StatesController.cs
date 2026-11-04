
using Assortment.Data;
using Assortment.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class StatesController : Controller
{

    private readonly AppDbContext _db;
    public StatesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(bool includeInactive = false, Guid? countryId = null, CancellationToken ct = default)
    {
        var q = _db.States.Include(s => s.Country).AsNoTracking().AsQueryable();
        if (!includeInactive) q = q.Where(s => s.IsActive);
        if (countryId is not null) q = q.Where(s => s.CountryId == countryId);
        ViewBag.IncludeInactive = includeInactive;
        ViewBag.CountryId = countryId;
        ViewBag.Countries = new SelectList(await _db.Countries.OrderBy(c => c.Name).ToListAsync(ct), "Id", "Name", countryId);
        return View(await q.OrderBy(s => s.Name).ToListAsync(ct));
    }

    public async Task<IActionResult> Create(CancellationToken ct = default)
    {
        await PopulateCountriesAsync(ct);
        return View(new State());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(State model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) { await PopulateCountriesAsync(ct, model.CountryId); return View(model); }
        model.Id = Guid.NewGuid(); model.CreatedAt = DateTimeOffset.UtcNow;
        _db.States.Add(model); await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Провинция «{model.Name}» создана";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct = default)
    {
        var s = await _db.States.FindAsync(new object[] { id }, ct);
        if (s is null) return NotFound();
        await PopulateCountriesAsync(ct, s.CountryId);
        return View(s);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, State model, CancellationToken ct = default)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) { await PopulateCountriesAsync(ct, model.CountryId); return View(model); }
        var e = await _db.States.FindAsync(new object[] { id }, ct);
        if (e is null) return NotFound();
        e.CountryId = model.CountryId; e.Name = model.Name; e.Translate = model.Translate;
        e.IsActive = model.IsActive; e.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Провинция «{e.Name}» обновлена";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct = default)
    {
        var s = await _db.States.Include(x => x.Country).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        return s is null ? NotFound() : View(s);
    }

    [HttpPost, ActionName("Deactivate"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateConfirmed(Guid id, CancellationToken ct = default)
    {
        var s = await _db.States.FindAsync(new object[] { id }, ct);
        if (s is null) return NotFound();
        if (await _db.Cities.AnyAsync(c => c.StateId == id, ct))
        {
            TempData["Error"] = "Нельзя деактивировать провинцию: в ней есть города";
            return RedirectToAction(nameof(Index));
        }
        s.IsActive = false; s.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Провинция «{s.Name}» деактивирована";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCountriesAsync(CancellationToken ct, Guid? selected = null)
    {
        ViewBag.Countries = new SelectList(
            await _db.Countries.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync(ct),
            "Id", "Name", selected);
    }
}
