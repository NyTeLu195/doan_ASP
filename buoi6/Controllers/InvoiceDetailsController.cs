using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using buoi6.Data;
using buoi6.Models;

namespace buoi6.Controllers
{
    public class InvoiceDetailsController : Controller
    {
        private readonly EshopContext _context;

        public InvoiceDetailsController(EshopContext context)
        {
            _context = context;
        }

        // GET: InvoiceDetails
        public async Task<IActionResult> Index()
        {
            var eshopContext = _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product);
            return View(await eshopContext.ToListAsync());
        }

        // GET: InvoiceDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetails = await _context.InvoiceDetails
                .Include(i => i.Invoice)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceDetails == null)
            {
                return NotFound();
            }

            return View(invoiceDetails);
        }

        // GET: InvoiceDetails/Create
        public IActionResult Create()
        {
            ViewData["InvoiceId"] = new SelectList(_context.Invoice, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id");
            return View();
        }

        // POST: InvoiceDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvoiceId,ProductId,Quantity,UnitPrice")] InvoiceDetails invoiceDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoiceDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvoiceId"] = new SelectList(_context.Invoice, "Id", "Id", invoiceDetails.InvoiceId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", invoiceDetails.ProductId);
            return View(invoiceDetails);
        }

        // GET: InvoiceDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetails = await _context.InvoiceDetails.FindAsync(id);
            if (invoiceDetails == null)
            {
                return NotFound();
            }
            ViewData["InvoiceId"] = new SelectList(_context.Invoice, "Id", "Id", invoiceDetails.InvoiceId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", invoiceDetails.ProductId);
            return View(invoiceDetails);
        }

        // POST: InvoiceDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvoiceId,ProductId,Quantity,UnitPrice")] InvoiceDetails invoiceDetails)
        {
            if (id != invoiceDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceDetailsExists(invoiceDetails.Id))
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
            ViewData["InvoiceId"] = new SelectList(_context.Invoice, "Id", "Id", invoiceDetails.InvoiceId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", invoiceDetails.ProductId);
            return View(invoiceDetails);
        }

        // GET: InvoiceDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetails = await _context.InvoiceDetails
                .Include(i => i.Invoice)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceDetails == null)
            {
                return NotFound();
            }

            return View(invoiceDetails);
        }

        // POST: InvoiceDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceDetails = await _context.InvoiceDetails.FindAsync(id);
            _context.InvoiceDetails.Remove(invoiceDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceDetailsExists(int id)
        {
            return _context.InvoiceDetails.Any(e => e.Id == id);
        }
    }
}
