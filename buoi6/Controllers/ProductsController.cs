using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using buoi6.Data;
using buoi6.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace buoi6.Controllers
{
    public class ProductsController : Controller
    {
        private readonly EshopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(EshopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index(string SKU, string ProductName, string Decription, string Price, string type)
        {
            var search = (from i in _context.Product.Include(p => p.ProductType)
                         select i);
            if (!String.IsNullOrEmpty(SKU))
            {
                search = search.Where(s => s.SKU.Contains(SKU));
            }
            if (!String.IsNullOrEmpty(ProductName))
            {
                search = search.Where(s => s.Name.Contains(ProductName));
            }
            if (!String.IsNullOrEmpty(Decription))
            {
                search = search.Where(s => s.Deccription.Contains(Decription));
            }
            if (!String.IsNullOrEmpty(Price))
            {
                search = search.Where(s => s.Price == Convert.ToInt32(Price));
            }
            if (!String.IsNullOrEmpty(type))
            {
                search = search.Where(s => s.ProductType.Name.Contains(type));
            }
            return View(await search.ToListAsync());
        }
        public async Task<IActionResult> ByPriceRange()
        {
            var eshopContext = _context.Product.Include(p => p.ProductType).Where(s=> s.Price >= 40000 && s.Price <= 60000);
           
            return View(await eshopContext.ToListAsync());
        }

        public async Task<IActionResult> ByProductType()
        {
            var eshopContext = _context.Product.Include(p => p.ProductType).Where(s=> s.ProductType.Name.Contains("Sách"));

            return View(await eshopContext.ToListAsync());
        }
        public async Task<IActionResult> ByStock()
        {
            var eshopContext = _context.Product.Include(p => p.ProductType).Where(s => s.Stock<10);

            return View(await eshopContext.ToListAsync());
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SKU,Name,Deccription,Price,Stock,ProductTypeId,Image,ImageFile,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();

                if (product.ImageFile != null)
                {
                    var filename = product.Id.ToString() + Path.GetExtension(product.ImageFile.FileName);
                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "product");
                    var filePath = Path.Combine(uploadPath, filename);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        product.ImageFile.CopyTo(fs);
                        fs.Flush();
                    }
                    product.Image = filename;
                    _context.Product.Update(product);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "Id", "Id", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "Id", "Id", product.ProductTypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SKU,Name,Deccription,Price,Stock,ProductTypeId,Image,ImageFile,Status")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    if (product.ImageFile != null)
                    {
                        var filename = product.Id.ToString() + Path.GetExtension(product.ImageFile.FileName);
                        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "product");
                        var filePath = Path.Combine(uploadPath, filename);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            product.ImageFile.CopyTo(fs);
                            fs.Flush();
                        }
                        product.Image = filename;
                        _context.Product.Update(product);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "Id", "Id", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
