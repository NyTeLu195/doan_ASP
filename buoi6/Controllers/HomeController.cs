using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using buoi6.Data;
using buoi6.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;

namespace buoi6.Controllers
{
    public class HomeController : Controller
    {

        private readonly EshopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(EshopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
          
              
           
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();
              
                int id = Int32.Parse(ViewBag.AccountID);
                Account account = _context.Account.Where(a =>a.Id==id ).FirstOrDefault();

                if (account.IsAdmin) return RedirectToAction("Index", "Admin");
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
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();
                int id = Int32.Parse(ViewBag.AccountID);
                Account account = _context.Account.Where(a => a.Id == id).FirstOrDefault();

                if (account.IsAdmin) return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        public IActionResult Blog_Single()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();
            }
            return View();
        }
        public async Task<IActionResult> Cart()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();
                int id = Int32.Parse(ViewBag.AccountID);
                Account account = _context.Account.Where(a => a.Id == id).FirstOrDefault();

                if (account.IsAdmin) return RedirectToAction("Index", "Admin");
                var eshopContext = _context.Cart.Include(c => c.Account).Include(c => c.Product).Where(c => c.Account.Username == SessionKeyName());
                return View(await eshopContext.ToListAsync());
            }
            return RedirectToAction("Login", "Accounts");
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
        public async Task<IActionResult> CheckoutAsync()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();
                int id = Int32.Parse(ViewBag.AccountID);
                Account account = _context.Account.Where(a => a.Id == id).FirstOrDefault();

                if (account.IsAdmin) return RedirectToAction("Index", "Admin");
                var eshopContext = _context.Cart.Include(c => c.Account).Include(c => c.Product).Where(c => c.Account.Username == SessionKeyName());
                return View(await eshopContext.ToListAsync());
            }
            return RedirectToAction("Login", "Accounts");
        }
        public IActionResult Contact()
        {
            return View();
        }
        public async Task<IActionResult> Product_Detail(int? id)
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();

                int AccountID = Int32.Parse(ViewBag.AccountID);
                Account account = _context.Account.Where(a => a.Id == AccountID).FirstOrDefault();

                if (account.IsAdmin) return RedirectToAction("Index", "Admin");
            }
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
        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.Id == id);
        }
        public async Task<IActionResult> EditAccount(int? id)
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();

                int AccountID = Int32.Parse(ViewBag.AccountID);
                Account accountadmin = _context.Account.Where(a => a.Id == id).FirstOrDefault();

                if (accountadmin.IsAdmin) return RedirectToAction("Index", "Admin");
            }
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(int id, [Bind("Id,Username,Password,Email,Phone,Address,FullName,IsAdmin,Avatar,ImageFile,TrangThai")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                    if (account.ImageFile != null)
                    {
                        var filename = account.Id.ToString() + Path.GetExtension(account.ImageFile.FileName);
                        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "account");
                        var filePath = Path.Combine(uploadPath, filename);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            account.ImageFile.CopyTo(fs);
                            fs.Flush();
                        }
                        account.Avatar = filename;
                        _context.Account.Update(account);
                        await _context.SaveChangesAsync();
               
                    
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                HttpContext.Response.Cookies.Append("AccountFullname", "", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
                HttpContext.Response.Cookies.Append("AccountFullname", account.FullName.ToString());
                return RedirectToAction("Profile");
            }
            return View(account);
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
        public async Task<IActionResult> Invoice()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();

                int id = Int32.Parse(ViewBag.AccountID);
                Account account = _context.Account.Where(a => a.Id == id).FirstOrDefault();

                if (account.IsAdmin) return RedirectToAction("Index", "Admin");
            }
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                var eshopContext = _context.Invoice.Include(i => i.Account).Where(c => c.Account.Username == SessionKeyName());
                return View(await eshopContext.ToListAsync());
            }
            return RedirectToAction("Login", "Accounts");
        }
        public async Task<IActionResult> InvoiceDetail(int id)
        {

            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();

                int AccountID = Int32.Parse(ViewBag.AccountID);
                Account account = _context.Account.Where(a => a.Id == AccountID).FirstOrDefault();

                if (account.IsAdmin) return RedirectToAction("Index", "Admin");
                var eshopContext = _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product).Where(i=>i.InvoiceId==id);
                return View(await eshopContext.ToListAsync());
            }
            return RedirectToAction("Login", "Accounts");
        }
        public async Task<IActionResult> Profile()
        {
            var id = -1;
            if (HttpContext.Request.Cookies.ContainsKey("AccountFullname"))
            {
                ViewBag.AccountUsername = HttpContext.Request.Cookies["AccountFullname"].ToString();
                ViewBag.AccountID = HttpContext.Request.Cookies["AccountID"].ToString();
                id = Int32.Parse(ViewBag.AccountID);
                Account account = _context.Account.Where(a => a.Id == id).FirstOrDefault();

                if (account.IsAdmin) return RedirectToAction("Index", "Admin");
                if (id == -1)
                {
                    return NotFound();
                }

                
                if (account == null)
                {
                    return NotFound();
                }

                return View(account);
            }
            return RedirectToAction("Login", "Accounts");
        }
    }
}