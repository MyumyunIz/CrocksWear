using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ShoeManager
    {
        ShoeContext shoeContext;
        public ShoeManager(ShoeContext shoeContext)
        {
            this.shoeContext = shoeContext;
        }
        public async Task CreateAsync(Shoe item)
        {
            await shoeContext.CreateAsync(item);
        }

        public async Task<Shoe> ReadAsync(int key, bool useNavigationalProperties = false)
        {
            return await shoeContext.ReadAsync(key, useNavigationalProperties);
        }

        public async Task<ICollection<Shoe>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            return await shoeContext.ReadAllAsync(useNavigationalProperties);
        }

        public async Task UpdateAsync(Shoe item, bool useNavigationalProperties = false)
        {
            await shoeContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await shoeContext.DeleteAsync(key);
        }
    }
}
