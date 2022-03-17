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
    public class CartsController : Controller
    {
        private readonly EshopContext _context;

        public CartsController(EshopContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()

        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountImage = HttpContext.Request.Cookies["AccountImage"].ToString();
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();
            }
            var eshopContext = _context.Cart.Include(c => c.Account).Include(c => c.Product);
            return View(await eshopContext.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountImage = HttpContext.Request.Cookies["AccountImage"].ToString();
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();
            }
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountImage = HttpContext.Request.Cookies["AccountImage"].ToString();
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountImage = HttpContext.Request.Cookies["AccountImage"].ToString();
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();
            }
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", cart.ProductId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountImage = HttpContext.Request.Cookies["AccountImage"].ToString();
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();
            }
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.Id == id);
        }
        
    }
}
