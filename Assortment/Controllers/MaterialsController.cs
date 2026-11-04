
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assortment.Domain.Entities;
using Assortment.Data;

public class MaterialsController : Controller
{

    private readonly AppDbContext _db;
    public MaterialsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(bool includeInactive = false, CancellationToken ct = default)
    {
        var q = _db.Materials.AsNoTracking().AsQueryable();
        if (!includeInactive) q = q.Where(m => m.IsActive);
        ViewBag.IncludeInactive = includeInactive;
        return View(await q.OrderBy(m => m.Name).ToListAsync(ct));
    }

    public IActionResult Create() => View(new Material());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Material model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return View(model);
        if (await _db.Materials.AnyAsync(m => m.Name == model.Name, ct))
        {
            ModelState.AddModelError(nameof(model.Name), "Материал с таким названием уже существует");
            return View(model);
        }
        model.Id = Guid.NewGuid(); model.CreatedAt = DateTimeOffset.UtcNow;
        _db.Materials.Add(model); await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Материал «{model.Name}» создан";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct = default)
    {
        var m = await _db.Materials.FindAsync(new object[] { id }, ct);
        return m is null ? NotFound() : View(m);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Material model, CancellationToken ct = default)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        if (await _db.Materials.AnyAsync(m => m.Name == model.Name && m.Id != id, ct))
        {
            ModelState.AddModelError(nameof(model.Name), "Материал с таким названием уже существует");
            return View(model);
        }
        var e = await _db.Materials.FindAsync(new object[] { id }, ct);
        if (e is null) return NotFound();
        e.Name = model.Name; e.Code = model.Code; e.Composition = model.Composition;
        e.IsActive = model.IsActive; e.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Материал «{e.Name}» обновлён";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct = default)
    {
        var m = await _db.Materials.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        return m is null ? NotFound() : View(m);
    }

    [HttpPost, ActionName("Deactivate"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateConfirmed(Guid id, CancellationToken ct = default)
    {
        var m = await _db.Materials.FindAsync(new object[] { id }, ct);
        if (m is null) return NotFound();
        m.IsActive = false; m.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Материал «{m.Name}» деактивирован";
        return RedirectToAction(nameof(Index));
    }
}
