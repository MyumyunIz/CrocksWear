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
    public class ManagerContext : IDB<Manager, string>
    {
        private readonly CrockDBContext dbContext;

        public ManagerContext(CrockDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Manager item)
        {
            try
            {
                dbContext.Managers.Add(item);
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
                Manager managerFromDb = await ReadAsync(key);

                if (managerFromDb != null)
                {
                    dbContext.Managers.Remove(managerFromDb);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("Manager with that id does not exist!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Manager>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Manager> query = dbContext.Managers;

                // Include navigational properties if needed
                if (useNavigationalProperties)
                {
                    query = query.Include(m => m.Shoes)
                                 .Include(m => m.Bills);
                }

                

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Manager> ReadAsync(string key, bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Manager> query = dbContext.Managers;

                // Include navigational properties if needed
                if (useNavigationalProperties)
                {
                    query = query.Include(m => m.Shoes)
                                 .Include(m => m.Bills);
                }

                

                return await query.FirstOrDefaultAsync(m => m.Id == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Manager item, bool useNavigationalProperties = false)
        {
            try
            {
                Manager managerFromDb = await ReadAsync(item.Id, useNavigationalProperties);

                // Update scalar properties
                managerFromDb.UserName = item.UserName;
                managerFromDb.Email = item.Email;
                managerFromDb.PhoneNumber = item.PhoneNumber;

                // Update navigational properties if requested
                if (useNavigationalProperties)
                {
                    List<Shoe> shoes = item.Shoes.ToList();
                    for (var i = 0; i < shoes.Count; i++)
                    {
                        Shoe shoeDb = await dbContext.Shoes.FindAsync(shoes[i].Id);
                        if (shoeDb != null)
                        {
                            shoes[i] = shoeDb;
                        }
                       

                    }
                    managerFromDb.Shoes = shoes;

                    List<Bill> bills = item.Bills.ToList();
                    for (var i = 0; i < bills.Count; i++)
                    {
                        Bill billDb = await dbContext.Bills.FindAsync(bills[i].Id);
                        if (billDb != null)
                        {
                            bills[i] = billDb;
                        }
                        

                    }
                    managerFromDb.Bills = bills;

                   
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
