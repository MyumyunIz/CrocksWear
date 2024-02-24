using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using BusinessLayer;


namespace ServiceLayer
{
    public class DbManager<T, K> where K : IConvertible
    {
        IDB<T, K> context;

        public DbManager(IDB<T, K> context)
        {
            this.context = context;
        }

        public async void CreateAsync(T item)
        {
            // Validation + Logging + Authorization + Authentication
            // + Error handling + Transformation from ViewModel to Model ...
            // Wrapper of Data Layer!
            await context.CreateAsync(item);
        }

        public async Task<T> ReadAsync(K key, bool useNavigationalProperties = false)
        {
             return await context.ReadAsync(key, useNavigationalProperties);
        }

        public async Task<ICollection<T>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            return await context.ReadAllAsync(useNavigationalProperties);
        }

        public async void UpdateAsync(T item, bool useNavigationalProperties = false)
        {
            await context.UpdateAsync(item, useNavigationalProperties);
        }

        public async void DeleteAsync(K key)
        {
            await context.DeleteAsync(key);
        }

    }
}
