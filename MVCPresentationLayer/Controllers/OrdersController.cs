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
    public class OrdersController : Controller
    {
        private readonly OrderManager orderManager;
        private readonly ShoeManager shoeManager;

        public OrdersController(OrderManager _orderManager, ShoeManager _shoeManager)
        {
            orderManager = _orderManager;
            this.shoeManager = _shoeManager;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await orderManager.ReadAllAsync(true));
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await orderManager.ReadAsync((int)id, true);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            await LoadNavigationalProperties();
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("Id,Quantity,Price,ShoeId,Status")] Order order)
        public async Task<IActionResult> Create(IFormCollection iformcollection)
        {
            await LoadNavigationalProperties();
            Shoe shoe = await shoeManager.ReadAsync(int.Parse(iformcollection["Shoe"]));
            Order order = new Order(shoe,int.Parse(iformcollection["Quantity"]), shoe.Price * int.Parse(iformcollection["Quantity"]), shoe.Price, OrderStatus.New);
            
            if (ModelState.IsValid)
            {
                await orderManager.CreateAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await orderManager.ReadAsync((int)id);
            if (order == null)
            {
                return NotFound();
            }
            await LoadNavigationalProperties();

            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection iformcollection)
        {
            Order oldOrder = await orderManager.ReadAsync(id,true);
            OrderStatus status = OrderStatus.New;
            if (Enum.TryParse(iformcollection["Status"], out OrderStatus parsedStatus))
            {
                status = parsedStatus; // Successfully parsed, assign the parsed enum value to status
            }
            
                Order order = new Order(oldOrder.Shoe, int.Parse(iformcollection["Quantity"]),decimal.Parse(iformcollection["Price"]), decimal.Parse(iformcollection["Shoeprice"]), status);
            order.Id = oldOrder.Id;


            if (id != oldOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await orderManager.UpdateAsync(order, true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await orderManager.ReadAsync((int)id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await orderManager.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OrderExists(int id)
        {
            return await orderManager.ReadAsync(id) is not null;
        }

        private async Task LoadNavigationalProperties()
        {
            ViewData["Shoes"] = new SelectList(await shoeManager.ReadAllAsync(), "Id", "Brand");
            var OrderStatusSelectList = Enum.GetValues(typeof(OrderStatus))
                                .Cast<OrderStatus>()
                                .Select(s => new SelectListItem
                                {
                                    Text = s.ToString(),
                                    Value = ((int)s).ToString() // If you want to use the enum's integer value
                                                                // Value = s.ToString() // If you want to use the enum's name as the value
                                });
            ViewData["Status"] = new SelectList(OrderStatusSelectList, "Value", "Text");

        }


        public async Task<ActionResult> GetPrice(int shoeId)
        {
            Shoe shoe = await shoeManager.ReadAsync(shoeId);
            return Json(new { success = true, price = shoe.Price });
        }
    }
}
