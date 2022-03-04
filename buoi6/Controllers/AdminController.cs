using buoi6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buoi6.Controllers
{
    public class AdminController : Controller
    {
        private readonly EshopContext _context;

        public AdminController(EshopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> InvoiceWeekAsync()
        {
            DateTime now = DateTime.Now;
            
                var dayOfWeek = now.DayOfWeek;
            DateTime monday =  DateTime.Now ;
                if (dayOfWeek == DayOfWeek.Sunday)
                {
                    //xét chủ nhật là đầu tuần thì thứ 2 là ngày kế tiếp nên sẽ tăng 1 ngày  
                    //return date.AddDays(1);  

                    // nếu xét chủ nhật là ngày cuối tuần  
                     monday= now.AddDays(-6);
                }
                else
                {
                    // nếu không phải thứ 2 thì lùi ngày lại cho đến thứ 2  
                    int offset = dayOfWeek - DayOfWeek.Monday;
                    monday = now.AddDays(-offset);
                }
                
            
            var eshopContext = _context.Invoice.Include(i => i.Account).Where(w => w.IsuedDate >= monday && w.IsuedDate <= now);
            return View(await eshopContext.ToListAsync());
        }


        
        public async Task<IActionResult> InvoiceTime(DateTime Batdau, DateTime Ketthuc)
        {


            if( Batdau!=null && Ketthuc!=null)
            {
                var eshopContext = _context.Invoice.Include(i => i.Account).Where(w => w.IsuedDate >= Batdau && w.IsuedDate <= Ketthuc);

                return View(await eshopContext.ToListAsync());
            }
            var invoices = _context.Invoice.Include(i => i.Account);

            return View(await invoices.ToListAsync());

        }


        public async Task<IActionResult> ProductQualityTime(DateTime Batdau, DateTime Ketthuc)
        {


            if (Batdau != null && Ketthuc != null)
            {
                var eshopContext = _context.Invoice .Include(i => i.Account).Where(w => w.IsuedDate >= Batdau && w.IsuedDate <= Ketthuc);

                return View(await eshopContext.ToListAsync());
            }
            var invoices = _context.Invoice.Include(i => i.Account);

            return View(await invoices.ToListAsync());

        }

    }
}
