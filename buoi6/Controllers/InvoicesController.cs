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
    public class InvoicesController : Controller
    {
        private readonly EshopContext _context;

        public InvoicesController(EshopContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var eshopContext = _context.Invoice.Include(i => i.Account);
            return View(await eshopContext.ToListAsync());
        }
        public async Task<IActionResult> ByTotalRange()
        {
            var eshopContext = _context.Invoice.Include(i => i.Account).Where(s=> s.Total >= 400000 && s.Total <= 600000);
            return View(await eshopContext.ToListAsync());
        }
        public async Task<IActionResult> ByAccountName()
        {
            var eshopContext = _context.Invoice.Include(i => i.Account).Where(s => s.Account.Username.Contains("john"));
            return View(await eshopContext.ToListAsync());
        }
        public async Task<IActionResult> ByAccountAddress()
        {
            var eshopContext = _context.Invoice.Include(i => i.Account).Where(s => s.Account.Address.Contains("Tp.Hồ Chí Minh"));
            return View(await eshopContext.ToListAsync());
        }
        public async Task<IActionResult> ByProduct()
        {
            var eshopContext = ( from i in _context.Invoice.Include(s=> s.Account)
                                 
                                 join ivd in _context.InvoiceDetails on  i.Id equals ivd.InvoiceId
                                 join p in _context.Product on ivd.ProductId equals p.Id
                                 where p.SKU.Contains("VIAZXR3Y24IY")
                                 select i );
           

            return View(await eshopContext.ToListAsync());
        }
        public async Task<IActionResult> ByProductType()
        {
            var eshopContext = (from i in _context.Invoice.Include(s => s.Account)

                                join ivd in _context.InvoiceDetails on i.Id equals ivd.InvoiceId
                                join p in _context.Product on ivd.ProductId equals p.Id
                                join T in _context.ProductType on p.ProductTypeId equals T.Id

                                where T.Name.Contains("Tiểu thuyết &Tự truyện")
                                select i);


            return View(await eshopContext.ToListAsync());
        }
        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,AccountId,IsuedDate,ShippingAddress,ShippingPhone,Total,Status")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", invoice.AccountId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", invoice.AccountId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,AccountId,IsuedDate,ShippingAddress,ShippingPhone,Total,Status")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", invoice.AccountId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.Id == id);
        }
    }
}
