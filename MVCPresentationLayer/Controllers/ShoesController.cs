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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO.Compression;

namespace MVCPresentationLayer.Controllers
{
    
    public class ShoesController : Controller
    {
        private readonly ShoeManager shoeManager;
        private readonly IdentityManager identityManager;
        private readonly ManagerManager managerManager;
        private readonly UserManager<User> userManager;


        public ShoesController(ShoeManager _shoeManager,IdentityManager _identityManager, ManagerManager _managerManager,UserManager<User> _userManager)
        {
            shoeManager = _shoeManager;
            identityManager = _identityManager;
            managerManager = _managerManager;
            userManager = _userManager;
        }

        // GET: Shoes
        public async Task<IActionResult> Index()
        {
            var a = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (a == null) return Redirect("/Identity/Account/Login");
            return View(await shoeManager.ReadAllAsync(true));
        }

        // GET: Shoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var shoe = await shoeManager.ReadAsync((int)id,true);
            if (shoe == null)
            {
                return NotFound();
            }

            return View(shoe);
        }

        // GET: Shoes/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Shoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("Id,Size,Brand,Model,Price,Color,Description,Icon_img")] Shoe shoe
        public async Task<IActionResult> Create(IFormCollection iformcollection,IFormFile Icon_img)
        {
            
            User user = await identityManager.ReadAsync(User.FindFirstValue(ClaimTypes.NameIdentifier),true);

            byte[] Icon_img_bytes = Array.Empty<byte>();
            if(Icon_img.Length> 0) 
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Icon_img.CopyToAsync(memoryStream);

                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152)
                    {

                        Icon_img_bytes = memoryStream.ToArray();
                       
                    }
                    
                }
            }



            Manager manager;
            if (!await userManager.IsInRoleAsync(user, Role.Manager.ToString()))
            {
                await userManager.AddToRoleAsync(user, Role.Manager.ToString());
                await userManager.RemoveFromRoleAsync(user, Role.User.ToString());

                //manager = new Manager();
                //await managerManager.CreateAsync(manager);
                //user.Manager = manager;
                //await userManager.UpdateAsync(user);
            }
            manager = await managerManager.ReadAsync(user.Manager.Id);
            Shoe shoe = new Shoe(int.Parse(iformcollection["Size"]), iformcollection["Brand"], iformcollection["Model"], decimal.Parse(iformcollection["Price"]), iformcollection["Color"], iformcollection["Description"],Icon_img_bytes,manager);
            if (ModelState.IsValid)
            {
                await shoeManager.CreateAsync(shoe);
                //await managerManager.UpdateAsync(manager,true);
                return RedirectToAction(nameof(Index));
            }
            return View(shoe);
        }

        // GET: Shoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoe = await shoeManager.ReadAsync((int)id);
            if (shoe == null)
            {
                return NotFound();
            }
            return View(shoe);
        }

        // POST: Shoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,IFormCollection iformcollection,IFormFile Icon_img)
        {
            User user = await identityManager.ReadAsync(User.FindFirstValue(ClaimTypes.NameIdentifier),true);
            Manager manager = await managerManager.ReadAsync(user.Manager.Id);
            Shoe shoe = new Shoe(int.Parse(iformcollection["Size"]), iformcollection["Brand"], iformcollection["Model"], decimal.Parse(iformcollection["Price"]), iformcollection["Color"], iformcollection["Description"], new byte[0], manager);
            shoe.Id = id;

            if (Icon_img != null)
            {
                byte[] Icon_img_bytes = Array.Empty<byte>();
                if (Icon_img.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Icon_img.CopyToAsync(memoryStream);

                        // Upload the file if less than 2 MB
                        if (memoryStream.Length < 2097152)
                        {

                            Icon_img_bytes = memoryStream.ToArray();

                            shoe.Icon_img = Icon_img_bytes; 
                        }

                    }
                }
            }
            
            if (id != shoe.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    await shoeManager.UpdateAsync(shoe,true);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await ShoeExists(shoe.Id))
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
            return View(shoe);
        }

        // GET: Shoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoe = await shoeManager.ReadAsync((int)id);
            if (shoe == null)
            {
                return NotFound();
            }

            return View(shoe);
        }

        // POST: Shoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await shoeManager.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ShoeExists(int id)
        {
            return await shoeManager.ReadAsync(id) is not null;
        }

        


    }
}
