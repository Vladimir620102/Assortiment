
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assortment.Domain.Entities;
using Assortment.Data;

public class CurrenciesController : Controller
{
    private readonly AppDbContext _db;

    public CurrenciesController(AppDbContext context)
    {
        _db = context;
    }

    // GET: CURRENCYS
    public async Task<IActionResult> Index()    
    {
        return View(await _db.Currencies.ToListAsync());
    }

    // GET: CURRENCYS/Details/5
    public async Task<IActionResult> Details(System.Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var currency = await _db.Currencies
            .FirstOrDefaultAsync(m => m.Id == id);
        if (currency == null)
        {
            return NotFound();
        }

        return View(currency);
    }

    // GET: CURRENCYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CURRENCYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,IsoCode,Name,Symbol,DecimalPlaces,IsActive,CreatedAt,UpdatedAt")] Currency currency)
    {
        if (ModelState.IsValid)
        {
            _db.Add(currency);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(currency);
    }

    // GET: CURRENCYS/Edit/5
    public async Task<IActionResult> Edit(System.Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var currency = await _db.Currencies.FindAsync(id);
        if (currency == null)
        {
            return NotFound();
        }
        return View(currency);
    }

    // POST: CURRENCYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(System.Guid? id, [Bind("Id,IsoCode,Name,Symbol,DecimalPlaces,IsActive,CreatedAt,UpdatedAt")] Currency currency)
    {
        if (id != currency.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _db.Update(currency);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyExists(currency.Id))
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
        return View(currency);
    }

    // GET: CURRENCYS/Delete/5
    public async Task<IActionResult> Delete(System.Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var currency = await _db.Currencies
            .FirstOrDefaultAsync(m => m.Id == id);
        if (currency == null)
        {
            return NotFound();
        }

        return View(currency);
    }

    // POST: CURRENCYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(System.Guid? id)
    {
        var currency = await _db.Currencies.FindAsync(id);
        if (currency != null)
        {
            _db.Currencies.Remove(currency);
        }

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CurrencyExists(System.Guid? id)
    {
        return _db.Currencies.Any(e => e.Id == id);
    }

    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct = default)
    {
        var c = await _db.Currencies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        return c is null ? NotFound() : View(c);
    }

    [HttpPost, ActionName("Deactivate"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateConfirmed(Guid id, CancellationToken ct = default)
    {
        var c = await _db.Currencies.FindAsync(new object[] { id }, ct);
        if (c is null) return NotFound();

        // Позже, когда появится Order:
        // if (await _db.Orders.AnyAsync(o => o.OrderCurrencyId == id || o.SettlementCurrencyId == id, ct))
        // {
        //     TempData["Error"] = "Нельзя деактивировать валюту: она используется в заказах";
        //     return RedirectToAction(nameof(Index));
        // }

        c.IsActive = false;
        c.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        TempData["Success"] = $"Валюта «{c.IsoCode}» деактивирована";
        return RedirectToAction(nameof(Index));
    }
}
