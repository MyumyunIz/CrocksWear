using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class IdentityManager
    {
        public IdentityContext context;

        public IdentityManager(IdentityContext context)
        {
            this.context = context;
        }

        public async Task<Tuple<IdentityResult,User>> CreateAsync(string username,string password, string email, Role role)
        {
            return await context.CreateUserAsync(username,password,email,role);
        }
        public async Task<User> ReadAsync(string key,bool useNavigationalProperties=false)
        {
            return await context.ReadUserAsync(key,useNavigationalProperties); 
        }
        public async Task<IEnumerable<User>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            return await context.ReadAllUsersAsync(useNavigationalProperties);
        }
        public async Task UpadateAsync(string id, string username)
        {
            await context.UpdateUserAsync(id, username);
        }
        public async Task DeleteAsync(string name)
        {
            await context.DeleteUserByNameAsync(name);
        }

    }
}
