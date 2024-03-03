using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class CartContext : IDB<Cart, int>
    {
        private readonly CrockDBContext dbContext;

        public CartContext(CrockDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Cart item)
        {
            try
            {
                dbContext.Carts.Add(item); // Assuming Carts is the DbSet<Cart>
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(int key)
        {
            try
            {
                Cart cartFromDb = await ReadAsync(key);

                if (cartFromDb != null)
                {
                    dbContext.Carts.Remove(cartFromDb);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("Cart with that id does not exist!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Cart>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Cart> query = dbContext.Carts;

                if (useNavigationalProperties)
                {
                    // Assuming Orders is a property you want to include
                    query = query.Include(c => c.Orders);
                }

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Cart> ReadAsync(int key, bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Cart> query = dbContext.Carts;

                if (useNavigationalProperties)
                {
                    query = query.Include(c => c.Orders);
                }

                return await query.FirstOrDefaultAsync(c => c.Id == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Cart item, bool useNavigationalProperties = false)
        {
            try
            {
                Cart cartFromDb = await ReadAsync(item.Id);


                cartFromDb.Price = item.Price;
                if (useNavigationalProperties && item.Orders != null)
                {
                    List<Order> orders = item.Orders.ToList();

                    for (var i = 0;i< orders.Count;i++)
                    {

                        Order existingOrder = await dbContext.Orders.FirstOrDefaultAsync(x=>x.Id==orders[i].Id);
                        if (existingOrder != null)
                        {
                            cartFromDb.Orders.Add(existingOrder);
                        }
                        else
                        {
                            cartFromDb.Orders.Add(orders[i]);
                        }
                    }
                }

                await dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal CalculatePriceOfOrders(ICollection<Order> orders)
        {
            decimal total = 0;
            if(orders != null)
            {
                foreach (Order order in orders)
                {
                    total += order.Price;
                }
            }
            return total;
        }
    }
}
