using buoi6.Data;
using buoi6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace buoi6.Controllers
{
    public class HomeController : Controller
    {
      
        private readonly EshopContext _context;
        
        public HomeController(EshopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
            }
            List<Product> products = _context.Product.Include(p => p.ProductType).ToList();
            List<ProductType> productType = _context.ProductType.ToList();

            Home home = new Home();
            home.listProduct = products;
            home.listProductType = productType;
            
            return View(home);
        }
        public IActionResult Blog()

        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
            }
            return View();
        }
        public IActionResult Blog_Single()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
            }
            return View();
        }
        public async Task<IActionResult> Cart()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                var eshopContext = _context.Cart.Include(c => c.Account).Include(c => c.Product).Where(c => c.Account.Username == SessionKeyName());
                return View(await eshopContext.ToListAsync());
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> DeleteCart(int? id)
        {
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
        [HttpPost, ActionName("DeleteCart")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCartConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Cart));
        }
        public IActionResult Checkout()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
            }
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public async Task<IActionResult> Product_Detail(int? id)
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
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Add(int id)
        {
            return Add(id, 1);
        }
        public IActionResult Remove(int id)
        {
            return Add(id, -1);
        }
        public IActionResult AddinHome(int id)
        {
            return AddinHome(id, 1);
        }
        public string SessionKeyName()
        {
            var name = HttpContext.Request.Cookies["AccountUsername"].ToString();
            return name;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int productId, int Quantity)
        {
            int accId = _context.Account.FirstOrDefault(a => a.Username == SessionKeyName()).Id;
            Cart cart = _context.Cart.FirstOrDefault(c => c.AccountId == accId && c.ProductId == productId);
            if (cart == null)
            {
                cart = new Cart();
                cart.AccountId = accId;
                cart.ProductId = productId;
                cart.Quantity = Quantity;
                _context.Cart.Add(cart);
            }
            else
            {
                cart.Quantity += Quantity;
            }
            _context.SaveChanges();
            return RedirectToAction("Cart");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddinHome(int productId, int Quantity)
        {
            int accId = _context.Account.FirstOrDefault(a => a.Username == SessionKeyName()).Id;
            Cart cart = _context.Cart.FirstOrDefault(c => c.AccountId == accId && c.ProductId == productId);
            if (cart == null)
            {
                cart = new Cart();
                cart.AccountId = accId;
                cart.ProductId = productId;
                cart.Quantity = Quantity;
                _context.Cart.Add(cart);
            }
            else
            {
                cart.Quantity += Quantity;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
