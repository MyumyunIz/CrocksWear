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

        public async Task<Tuple<IdentityResult,User>> CreateUserAsync(string username, string password, string email, Role role)
        {
            try
            {
                User user = new User(username, email);
                IdentityResult result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    return new Tuple<IdentityResult,User>(result,user);
                }

                if (role == Role.Administrator)
                {
                    await userManager.AddToRoleAsync(user, Role.Administrator.ToString());
                }
                else
                {
                    await userManager.AddToRoleAsync(user, Role.User.ToString());
                }
                return new Tuple<IdentityResult,User>(IdentityResult.Success,user);
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

        public async Task<User> ReadUserAsync(string key,bool useNavigationalProperties)
        {
            try
            {
                if(useNavigationalProperties)
                {
                    return  await context.Users.Include(x => x.Orders).SingleOrDefaultAsync(x=>x.Id==key);
                }
                return await userManager.FindByIdAsync(key);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<User>> ReadAllUsersAsync(bool useNavigationalProperties)
        {
            try
            {
                if(useNavigationalProperties)
                {
                    return await context.Users.Include(x => x.Orders).ToListAsync();
                }
                return await context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateUserAsync(string id, string username)
        {
            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    User user = await context.Users.FindAsync(id);
                    user.UserName = username;
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
