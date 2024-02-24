using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace DataLayer
{
    public class UserContext : IDB<User, string>
    {
        private readonly CrockDBContext dbContext;

        public UserContext(CrockDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(User item)
        {
            try
            {
                dbContext.Users.Add(item);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(string key)
        {
            try
            {
                User userFromDb = await ReadAsync(key);

                if (userFromDb != null)
                {
                    dbContext.Users.Remove(userFromDb);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("User with that id does not exist!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<User>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<User> query = dbContext.Users;

                // Include navigational properties if needed
                if (useNavigationalProperties)
                {
                    query = query.Include(u => u.Orders);
                }

                

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> ReadAsync(string key, bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<User> query = dbContext.Users;

                // Include navigational properties if needed
                if (useNavigationalProperties)
                {
                    query = query.Include(u => u.Orders);
                }

                

                return await query.FirstOrDefaultAsync(u => u.Id == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(User item, bool useNavigationalProperties = false)
        {
            try
            {
                User userFromDb = await ReadAsync(item.Id,useNavigationalProperties);

               userFromDb.UserName = item.UserName;
                userFromDb.Email = item.Email;

                
                if (useNavigationalProperties)
                {
                    var originalOrders = userFromDb.Orders.ToList();
                    var newOrders = item.Orders.ToList();


                    if (item.Orders != null)
                    {


                        for (var i = 0; i < newOrders.Count; i++)
                        {
                            Order orderFromDb = await dbContext.Orders.FindAsync(newOrders[i].Id);
                            if (orderFromDb != null)
                            {
                                newOrders[i] = orderFromDb;
                            }
                        }
                        userFromDb.Orders = newOrders;
                    }
                    else
                    {
                        userFromDb.Orders = null;
                    }
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
