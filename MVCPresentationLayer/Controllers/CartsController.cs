using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;
using DataLayer;
using ServiceLayer;

namespace MVCPresentationLayer.Controllers
{
    public class CartsController : Controller
    {
        private readonly CartManager cartManager;
        private readonly OrderManager orderManager;

        public CartsController(CartManager _cartManager, OrderManager _orderManager)
        {
            cartManager = _cartManager;
            orderManager = _orderManager;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            return View(await cartManager.ReadAllAsync(true));
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await cartManager.ReadAsync((int)id,true);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        //// GET: Carts/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Price")] Cart cart)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        cartManager.Add(cart);
        //        await cartManager.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(cart);
        //}

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await cartManager.ReadAsync((int)id,true);
            if (cart == null)
            {
                return NotFound();
            }
            await LoadNavigationalProperties();
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection iformcollection)
        {
            Cart cart = await cartManager.ReadAsync(id,true);

            var ordersids_list = iformcollection["Orders"].Select(int.Parse).ToArray();
            List<Order> orders = new List<Order>();
            for(var i = 0; i < ordersids_list.Length;i++)
            {
                Order order = await orderManager.ReadAsync(ordersids_list[i]);
                if(order != null)
                {
                    orders.Add(order);
                }
            }

            cart.Orders = orders;
            cart.Price = decimal.Parse( iformcollection["Price"]);

            if (ModelState.IsValid)
            {
                try
                {
                    await cartManager.UpdateAsync(cart,true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CartExists(cart.Id))
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
            return View(cart);
        }

        //// GET: Carts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cart = await cartManager.ReadAsync((int)id);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cart);
        //}

        //// POST: Carts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    await cartManager.DeleteAsync(id);
        //    return RedirectToAction(nameof(Index));
        //}

        private async Task<bool> CartExists(int id)
        {
          return (await cartManager.ReadAsync(id) is not null);
        }
        private async Task LoadNavigationalProperties()
        {
            ViewData["Orders"] = await orderManager.ReadAllAsync(true);
        }
        [HttpPost]
        public async Task<ActionResult> GetPrice([FromBody] string orderIds)
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
            return Json(new { success = false });
        }
    }
}
