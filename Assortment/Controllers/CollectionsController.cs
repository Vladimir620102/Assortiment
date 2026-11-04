
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assortment.Domain.Entities;
using Assortment.Data;

public class CollectionsController : Controller
{
    private readonly AppDbContext _context;

    public CollectionsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: COLLECTIONS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Collections.ToListAsync());
    }

    // GET: COLLECTIONS/Details/5
    public async Task<IActionResult> Details(System.Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var collection = await _context.Collections
            .FirstOrDefaultAsync(m => m.Id == id);
        if (collection == null)
        {
            return NotFound();
        }

        return View(collection);
    }

    // GET: COLLECTIONS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: COLLECTIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,BrandId,Brand,SeasonId,Season,Name,Code,IsActive,CreatedAt,UpdatedAt")] Collection collection)
    {
        if (ModelState.IsValid)
        {
            _context.Add(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(collection);
    }

    // GET: COLLECTIONS/Edit/5
    public async Task<IActionResult> Edit(System.Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var collection = await _context.Collections.FindAsync(id);
        if (collection == null)
        {
            return NotFound();
        }
        return View(collection);
    }

    // POST: COLLECTIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(System.Guid? id, [Bind("Id,BrandId,Brand,SeasonId,Season,Name,Code,IsActive,CreatedAt,UpdatedAt")] Collection collection)
    {
        if (id != collection.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(collection);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollectionExists(collection.Id))
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
        return View(collection);
    }

    // GET: COLLECTIONS/Delete/5
    public async Task<IActionResult> Delete(System.Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var collection = await _context.Collections
            .FirstOrDefaultAsync(m => m.Id == id);
        if (collection == null)
        {
            return NotFound();
        }

        return View(collection);
    }

    // POST: COLLECTIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(System.Guid? id)
    {
        var collection = await _context.Collections.FindAsync(id);
        if (collection != null)
        {
            _context.Collections.Remove(collection);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CollectionExists(System.Guid? id)
    {
        return _context.Collections.Any(e => e.Id == id);
    }
}
