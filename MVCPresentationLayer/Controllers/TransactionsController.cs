using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using ServiceLayer;
using System.Security.Cryptography.Xml;
using System.Security.Claims;
using System.Xml.Schema;
using static NuGet.Packaging.PackagingConstants;

namespace MVCPresentationLayer.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly TransactionManager transactionManager;
        private readonly UserManager<User> userManager;
        private readonly OrderManager orderManager;
        private readonly IdentityManager identityManager;

        public TransactionsController(TransactionManager transactionManager, UserManager<User> userManager, OrderManager orderManager,IdentityManager _identityManager)
        {
            this.transactionManager = transactionManager;
            this.userManager = userManager;
            this.orderManager = orderManager;
            this.identityManager = _identityManager;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var a = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (a == null) return Redirect("/Identity/Account/Login");
            return View(await transactionManager.ReadAllAsync(true));
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await transactionManager.ReadAsync((int)id, true);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public async Task<IActionResult> Create()
        {
            await LoadNavigationalProperties();
            return View();
        }

        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection iformcollection)
        {
            User user = await identityManager.ReadAsync(User.FindFirstValue(ClaimTypes.NameIdentifier),true);
            List<Order> orders = new List<Order>();
            var orderslist = iformcollection["Orders"].Select(int.Parse).ToArray();
            decimal total = 0;
            for(var i = 0; i<orderslist.Length; i++)
            {
                Order order = await orderManager.ReadAsync(orderslist[i],true);
                if(order != null)
                {
                    orders.Add(order);
                    total += order.Price;
                }
            }

            Transaction transaction = new Transaction(user, iformcollection["Address"], iformcollection["Bankcard"],total,orders); ;
            if (ModelState.IsValid)
            {
                await transactionManager.CreateAsync(transaction);
                return RedirectToAction(nameof(Index));
            }
            await LoadNavigationalProperties(); // Reload navigational properties if there's a need to return to the view
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await transactionManager.ReadAsync((int)id,true);
            if (transaction == null)
            {
                return NotFound();
            }

            await LoadNavigationalProperties();
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,IFormCollection iformcollection)
        {
            Transaction transaction = await transactionManager.ReadAsync(id,true);
            transaction.Address = iformcollection["Address"];
            transaction.Price = decimal.Parse(iformcollection["Price"]);
            transaction.BankCard = iformcollection["Bankcard"];


            List<Order> orders = new List<Order>();
            var orderslist = iformcollection["Orders"].Select(int.Parse).ToArray();
            for (var i = 0; i < orderslist.Length; i++)
            {
                Order order = await orderManager.ReadAsync(orderslist[i], true);
                if (order != null)
                {
                    orders.Add(order);
                }
            }
            transaction.Orders = orders;

            if (ModelState.IsValid)
            {
                try
                {
                    await transactionManager.UpdateAsync(transaction,true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TransactionExists(transaction.Id))
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
            await LoadNavigationalProperties();
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await transactionManager.ReadAsync((int)id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await transactionManager.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TransactionExists(int id)
        {
            return await transactionManager.ReadAsync(id) is not null;
        }

        private async Task LoadNavigationalProperties()
        {
            // Example for loading navigational properties
            //ViewData["Orders"] = new SelectList(await orderManager.ReadAllAsync(true), "Id", "Id"); 
            ViewData["Orders"] = await orderManager.ReadAllAsync(true);

        }
        [HttpPost]
        public async Task<ActionResult> GetPrice([FromBody]string orderIds)
        {
            if (orderIds != null)
            {
                var orderId_arr = orderIds.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                decimal total = 0;
                for (var i = 0; i < orderId_arr.Length; i++)
                {
                    Order order = await orderManager.ReadAsync(orderId_arr[i]);
                    if (order != null)
                    {
                        total += order.Price;
                    }
                }
                return Json(new { success = true, price = total });
            }
            return Json(new {success=false});
        }
    }
}
