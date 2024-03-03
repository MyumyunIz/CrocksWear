using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CartManager
    {
        private readonly CartContext cartContext;

        public CartManager(CartContext cartContext)
        {
            this.cartContext = cartContext;
        }

        public async Task CreateAsync(Cart item)
        {
            await cartContext.CreateAsync(item);
        }

        public async Task<Cart> ReadAsync(int key, bool useNavigationalProperties = false)
        {
            return await cartContext.ReadAsync(key, useNavigationalProperties);
        }

        public async Task<ICollection<Cart>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            return await cartContext.ReadAllAsync(useNavigationalProperties);
        }

        public async Task UpdateAsync(Cart item, bool useNavigationalProperties = false)
        {
            await cartContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await cartContext.DeleteAsync(key);
        }

        public decimal CalculatePriceOfOrders(ICollection<Order> orders)
        {
            return cartContext.CalculatePriceOfOrders(orders);
        }
    }
}
