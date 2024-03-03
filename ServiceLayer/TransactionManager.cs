using BusinessLayer;
using DataLayer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class TransactionManager
    {
        TransactionContext transactionContext;

        public TransactionManager(TransactionContext transactionContext)
        {
            this.transactionContext = transactionContext;
        }

        public async Task CreateAsync(Transaction item)
        {
            await transactionContext.CreateAsync(item);
        }

        public async Task<Transaction> ReadAsync(int key, bool useNavigationalProperties = false)
        {
            return await transactionContext.ReadAsync(key, useNavigationalProperties);
        }

        public async Task<ICollection<Transaction>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            return await transactionContext.ReadAllAsync(useNavigationalProperties);
        }

        public async Task UpdateAsync(Transaction item, bool useNavigationalProperties = false)
        {
            await transactionContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await transactionContext.DeleteAsync(key);
        }
        
    }
}
