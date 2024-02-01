using BusinessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class IdentityContext 
    {
        UserManager<User> userManager;
        CrockDBContext context;

        public IdentityContext(UserManager<User> userManager, CrockDBContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public async Task SeedDataAsync(string adminPass, string adminEmail)
        {
            //await context.Database.MigrateAsync();

            int userRoles = await context.UserRoles.CountAsync();

            if (userRoles == 0)
            {
                await ConfigureAdminAccountAsync(adminPass, adminEmail);
            }
        }

        public async Task ConfigureAdminAccountAsync(string password, string email)
        {
            User adminIdentityUser = await context.Users.FirstAsync();

            if (adminIdentityUser != null)
            {
                await userManager.AddToRoleAsync(adminIdentityUser, Role.Administrator.ToString());
                await userManager.AddPasswordAsync(adminIdentityUser, password);
                await userManager.SetEmailAsync(adminIdentityUser, email);
            }
        }





        //CRUD

        public async Task CreateUserAsync(string username, string password, string email, string name, Role role)
        {
            try
            {
                User user = new User(username, email, name);
                IdentityResult result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new ArgumentException(result.Errors.First().Description);
                }

                if (role == Role.Administrator)
                {
                    await userManager.AddToRoleAsync(user, Role.Administrator.ToString());
                }
                else
                {
                    await userManager.AddToRoleAsync(user, Role.User.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> LogInUserAsync(string username, string password)
        {
            try
            {
                User user = await userManager.FindByNameAsync(username);

                if (user == null)
                {
                    return null;
                }

                IdentityResult result = await userManager.PasswordValidators[0].ValidateAsync(userManager, user, password);

                if (result.Succeeded)
                {
                    return await context.Users.FindAsync(user.Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> ReadUserAsync(string key)
        {
            try
            {
                return await userManager.FindByIdAsync(key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<User>> ReadAllUsersAsync()
        {
            try
            {
                return await context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateUserAsync(string id, string username, string name)
        {
            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    User user = await context.Users.FindAsync(id);
                    user.UserName = username;
                    user.Name = name;
                    await userManager.UpdateAsync(user);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteUserByNameAsync(string name)
        {
            try
            {
                User user = await FindUserByNameAsync(name);

                if (user == null)
                {
                    throw new InvalidOperationException("User not found for deletion!");
                }

                await userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> FindUserByNameAsync(string name)
        {
            try
            {
                // Identity return Null if there is no user!
                return await userManager.FindByNameAsync(name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
