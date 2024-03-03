using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class UserManager
    {
        UserContext userContext; // Assuming UserContext is your DbContext or equivalent for handling users

        public UserManager(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public async Task CreateAsync(User item)
        {
            await userContext.CreateAsync(item);
        }

        public async Task<User> ReadAsync(string key, bool useNavigationalProperties = false)
        {
            return await userContext.ReadAsync(key, useNavigationalProperties);
        }

        public async Task<ICollection<User>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            return await userContext.ReadAllAsync(useNavigationalProperties);
        }

        public async Task UpdateAsync(User item, bool useNavigationalProperties = false)
        {
            await userContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(string key)
        {
            await userContext.DeleteAsync(key);
        }
    }
}
