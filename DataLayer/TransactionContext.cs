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
    public class TransactionContext : IDB<Transaction, int>
    {
        private readonly CrockDBContext dbContext;

        public TransactionContext(CrockDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Transaction item)
        {
            try
            {
                dbContext.Bills.Add(item);
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
                Transaction billFromDb = await ReadAsync(key,true);

                //cascade delete error solution
                List<Order> orders = billFromDb.Orders;
                if(orders != null)
                {
                    for(var i = 0; i < orders.Count; i++)
                    {
                        orders[i].Transaction = null;
                    }
                }



                if (billFromDb != null)
                {
                    dbContext.Bills.Remove(billFromDb);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("Bill with that id does not exist!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Transaction>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Transaction> query = dbContext.Bills;

                // Include navigational properties if needed
                if (useNavigationalProperties)
                {
                    query = query.Include(b => b.User)
                                 .Include(b => b.Orders);
                }


                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Transaction> ReadAsync(int key, bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Transaction> query = dbContext.Bills;

                // Include navigational properties if needed
                if (useNavigationalProperties)
                {
                    query = query.Include(b => b.User)
                                 .Include(b => b.Orders);
                }

                
                return await query.FirstOrDefaultAsync(b => b.Id == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Transaction item, bool useNavigationalProperties = false)
        {
            try
            {
                Transaction billFromDb = await ReadAsync(item.Id,useNavigationalProperties);

                // Update scalar properties
                billFromDb.Address = item.Address;
                billFromDb.BankCard = item.BankCard;
                billFromDb.Price = item.Price;

                // Update navigational property if requested
                if (useNavigationalProperties)
                {
                    User userDB = await dbContext.Users.FindAsync(item.User.Id);
                    if (userDB != null)
                    {
                        billFromDb.User = userDB;
                    }
                    else
                    {
                        billFromDb.User = item.User;
                    }

                    List<Order> orders = item.Orders.ToList();
                    for(var i = 0; i < orders.Count;i++)
                    {
                        Order orderDb = await dbContext.Orders.FindAsync(orders[i].Id);
                        if(orderDb != null)
                        {
                            orders[i] = orderDb;
                        }
                        

                    }
                    billFromDb.Orders = orders;

                
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
