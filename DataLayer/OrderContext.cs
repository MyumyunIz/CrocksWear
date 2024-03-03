﻿using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace DataLayer
{
    public class OrderContext : IDB<Order, int>
    {
        private readonly CrockDBContext dbContext;

        public OrderContext(CrockDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Order item)
        {
            try
            {
                dbContext.Orders.Add(item);
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
                Order orderFromDb = await ReadAsync(key);

                if (orderFromDb != null)
                {
                    dbContext.Orders.Remove(orderFromDb);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("Order with that id does not exist!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Order>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Order> query = dbContext.Orders;

                // Include navigational properties if needed
                if (useNavigationalProperties)
                {
                    query = query.Include(o => o.Shoe)
                                 .Include(o => o.Transaction);
                }

                // Set read-only option if needed
                

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Order> ReadAsync(int key, bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Order> query = dbContext.Orders;

                // Include navigational properties if needed
                if (useNavigationalProperties)
                {
                    query = query.Include(o => o.Shoe)
                                 .Include(o => o.Transaction);
                }

                
                return await query.FirstOrDefaultAsync(o => o.Id == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Order item, bool useNavigationalProperties = false)
        {
            try
            {
                Order orderFromDb = await ReadAsync(item.Id, useNavigationalProperties);

                
                orderFromDb.Quantity = item.Quantity;
                orderFromDb.Price = item.Price;
                orderFromDb.Shoeprice = item.Shoeprice;
                orderFromDb.Status = item.Status;

                
                if (useNavigationalProperties)
                {
                    Shoe shoeDB = await dbContext.Shoes.FindAsync(item.Shoe.Id);
                    if(shoeDB!= null)
                    {
                        orderFromDb.Shoe = shoeDB;
                    }
                    else
                    {
                        orderFromDb.Shoe = item.Shoe;
                    }
                    if (item.Transaction != null)
                    {
                        Transaction bill = await dbContext.Bills.FindAsync(item.Transaction.Id);
                        if (bill != null)
                        {
                            orderFromDb.Transaction = bill;
                        }
                        else
                        {
                            orderFromDb.Transaction = item.Transaction;
                        }
                        
                    }

                    else
                    {
                        orderFromDb.Transaction = null;
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
