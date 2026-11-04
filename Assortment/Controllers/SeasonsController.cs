using Assortment.Data;
using Assortment.Domain.Entities;
using Assortment.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assortment.Controllers
{
    public class SeasonsController : Controller
    {
        private readonly AppDbContext _db;

        public SeasonsController(AppDbContext db) => _db = db;

        // GET: /Seasons
        public async Task<IActionResult> Index(
            string? search,
            SeasonKind? kind,
            bool includeInactive = false,
            CancellationToken ct = default)
        {
            var query = _db.Seasons.AsNoTracking().AsQueryable();

            if (!includeInactive)
                query = query.Where(s => s.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(s => s.Name.Contains(search));

            if (kind is not null)
                query = query.Where(s => s.Kind == kind);

            var items = await query
                .OrderBy(s => s.Kind)
                .ThenBy(s => s.Name)
                .ToListAsync(ct);

            ViewBag.Search = search;
            ViewBag.Kind = kind;
            ViewBag.IncludeInactive = includeInactive;

            return View(items);
        }

        // GET: /Seasons/Details/{id}
        public async Task<IActionResult> Details(Guid id, CancellationToken ct = default)
        {
            var season = await _db.Seasons
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, ct);

            return season is null ? NotFound() : View(season);
        }

        // GET: /Seasons/Create
        public IActionResult Create() => View(new Season());

        // POST: /Seasons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Season model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _db.Seasons.AnyAsync(
                    s => s.Kind == model.Kind && s.Name == model.Name, ct))
            {
                ModelState.AddModelError(nameof(model.Name),
                    $"Сезон «{model.Kind.GetDisplayName()} — {model.Name}» уже существует");
                return View(model);
            }

            model.Id = Guid.NewGuid();
            model.CreatedAt = DateTimeOffset.UtcNow;

            _db.Seasons.Add(model);
            await _db.SaveChangesAsync(ct);

            TempData["Success"] = $"Сезон «{model.Name}» создан";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Seasons/Edit/{id}
        public async Task<IActionResult> Edit(Guid id, CancellationToken ct = default)
        {
            var season = await _db.Seasons.FindAsync(new object[] { id }, ct);
            return season is null ? NotFound() : View(season);
        }

        // POST: /Seasons/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Season model, CancellationToken ct = default)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            if (await _db.Seasons.AnyAsync(
                    s => s.Kind == model.Kind && s.Name == model.Name && s.Id != id, ct))
            {
                ModelState.AddModelError(nameof(model.Name),
                    $"Сезон «{model.Kind.GetDisplayName()} — {model.Name}» уже существует");
                return View(model);
            }

            var entity = await _db.Seasons.FindAsync(new object[] { id }, ct);
            if (entity is null)
                return NotFound();

            entity.Kind = model.Kind;
            entity.Name = model.Name;
            entity.Translate = model.Translate;
            entity.ExternalCode = model.ExternalCode;
            entity.LabelName = model.LabelName;
            entity.ExpireDate = model.ExpireDate;
            entity.IsActive = model.IsActive;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);

            TempData["Success"] = $"Сезон «{entity.Name}» обновлён";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Seasons/Deactivate/{id}
        public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct = default)
        {
            var season = await _db.Seasons
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, ct);

            return season is null ? NotFound() : View(season);
        }

        // POST: /Seasons/Deactivate/{id}
        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateConfirmed(Guid id, CancellationToken ct = default)
        {
            var season = await _db.Seasons.FindAsync(new object[] { id }, ct);
            if (season is null)
                return NotFound();

            // Проверка: есть ли модели этого сезона.
            // Таблица Models появится позже — сейчас проверку можно закомментировать,
            // но структуру оставить, чтобы не забыть, когда модели появятся.
            //
            // var modelCount = await _db.Models.CountAsync(m => m.SeasonId == id, ct);
            // if (modelCount > 0)
            // {
            //     TempData["Error"] = $"Нельзя деактивировать сезон: в нём {modelCount} моделей";
            //     return RedirectToAction(nameof(Index));
            // }

            season.IsActive = false;
            season.UpdatedAt = DateTimeOffset.UtcNow;
            await _db.SaveChangesAsync(ct);

            TempData["Success"] = $"Сезон «{season.Name}» деактивирован";
            return RedirectToAction(nameof(Index));
        }
    }
}
