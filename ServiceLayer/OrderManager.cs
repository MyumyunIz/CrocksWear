using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class OrderManager
    {
        OrderContext orderContext;

        public OrderManager(OrderContext orderContext)
        {
            this.orderContext = orderContext;
        }

        public async Task CreateAsync(Order item)
        {
            await orderContext.CreateAsync(item);
        }

        public async Task<Order> ReadAsync(int key, bool useNavigationalProperties = false)
        {
            return await orderContext.ReadAsync(key, useNavigationalProperties);
        }

        public async Task<ICollection<Order>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            return await orderContext.ReadAllAsync(useNavigationalProperties);
        }

        public async Task UpdateAsync(Order item, bool useNavigationalProperties = false)
        {
            await orderContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await orderContext.DeleteAsync(key);
        }
    }
}
