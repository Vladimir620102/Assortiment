
using Assortment.Data;
using Assortment.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ModelsController : Controller
{

    private readonly AppDbContext _db;
    public ModelsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(bool includeInactive = false, Guid? collectionId = null, CancellationToken ct = default)
    {
        var q = _db.Models.Include(m => m.Collection).ThenInclude(c => c.Brand)
            .AsNoTracking().AsQueryable();
        if (!includeInactive) q = q.Where(m => m.IsActive);
        if (collectionId is not null) q = q.Where(m => m.CollectionId == collectionId);
        ViewBag.IncludeInactive = includeInactive;
        ViewBag.CollectionId = collectionId;
        ViewBag.Collections = new SelectList(
            await _db.Collections.Include(c => c.Brand).OrderBy(c => c.Name).ToListAsync(ct),
            "Id", "Name", collectionId);
        return View(await q.OrderBy(m => m.Code).ToListAsync(ct));
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken ct = default)
    {
        var m = await _db.Models
            .Include(x => x.Collection).ThenInclude(c => c.Brand)
            .Include(x => x.Collection).ThenInclude(c => c.Season)
            .Include(x => x.Materials).ThenInclude(mm => mm.Material)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        return m is null ? NotFound() : View(m);
    }

    public async Task<IActionResult> Create(CancellationToken ct = default)
    {
        await PopulateCollectionsAsync(ct);
        return View(new Model());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Model model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) { await PopulateCollectionsAsync(ct, model.CollectionId); return View(model); }
        if (await _db.Models.AnyAsync(m => m.Code == model.Code, ct))
        {
            ModelState.AddModelError(nameof(model.Code), "Модель с таким артикулом уже существует");
            await PopulateCollectionsAsync(ct, model.CollectionId); return View(model);
        }
        model.Id = Guid.NewGuid(); model.CreatedAt = DateTimeOffset.UtcNow;
        _db.Models.Add(model); await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Модель «{model.Code}» создана";
        return RedirectToAction(nameof(Edit), new { id = model.Id });
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct = default)
    {
        var m = await _db.Models
            .Include(x => x.Materials).ThenInclude(mm => mm.Material)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        if (m is null) return NotFound();
        await PopulateCollectionsAsync(ct, m.CollectionId);
        await PopulateMaterialsForAddingAsync(ct);
        return View(m);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Model model, CancellationToken ct = default)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) { await PopulateCollectionsAsync(ct, model.CollectionId); return View(model); }
        if (await _db.Models.AnyAsync(m => m.Code == model.Code && m.Id != id, ct))
        {
            ModelState.AddModelError(nameof(model.Code), "Модель с таким артикулом уже существует");
            await PopulateCollectionsAsync(ct, model.CollectionId); return View(model);
        }
        var e = await _db.Models
            .Include(x => x.Materials).ThenInclude(mm => mm.Material)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        if (e is null) return NotFound();
        e.Code = model.Code; e.Name = model.Name; e.CollectionId = model.CollectionId;
        e.Gender = model.Gender; e.IsActive = model.IsActive;
        e.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Модель «{e.Code}» обновлена";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddMaterial(Guid id, Guid materialId, ComponentType componentType, CancellationToken ct = default)
    {
        var model = await _db.Models.FindAsync(new object[] { id }, ct);
        if (model is null) return NotFound();
        if (await _db.ModelMaterials.AnyAsync(mm => mm.ModelId == id && mm.MaterialId == materialId && mm.ComponentType == componentType, ct))
        {
            TempData["Error"] = "Этот материал уже указан для этого компонента";
            return RedirectToAction(nameof(Edit), new { id });
        }
        _db.ModelMaterials.Add(new ModelMaterial
        {
            ModelId = id,
            MaterialId = materialId,
            ComponentType = componentType
        });
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = "Материал добавлен";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveMaterial(Guid id, Guid modelMaterialId, CancellationToken ct = default)
    {
        var mm = await _db.ModelMaterials.FirstOrDefaultAsync(x => x.Id == modelMaterialId && x.ModelId == id, ct);
        if (mm is null) return NotFound();
        _db.ModelMaterials.Remove(mm);
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = "Материал удалён";
        return RedirectToAction(nameof(Edit), new { id });
    }

    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct = default)
    {
        var m = await _db.Models.Include(x => x.Collection).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        return m is null ? NotFound() : View(m);
    }

    [HttpPost, ActionName("Deactivate"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateConfirmed(Guid id, CancellationToken ct = default)
    {
        var m = await _db.Models.FindAsync(new object[] { id }, ct);
        if (m is null) return NotFound();
        m.IsActive = false; m.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Модель «{m.Code}» деактивирована";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCollectionsAsync(CancellationToken ct, Guid? selected = null)
    {
        ViewBag.Collections = new SelectList(
            await _db.Collections.Include(c => c.Brand).Where(c => c.IsActive)
                .OrderBy(c => c.Name).ToListAsync(ct),
            "Id", "Name", selected);
    }

    private async Task PopulateMaterialsForAddingAsync(CancellationToken ct)
    {
        ViewBag.AllMaterials = new SelectList(
            await _db.Materials.Where(m => m.IsActive).OrderBy(m => m.Name).ToListAsync(ct),
            "Id", "Name");
    }
}
