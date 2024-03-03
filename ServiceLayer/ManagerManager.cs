using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ManagerManager
    {
        ManagerContext managerContext;

        public ManagerManager(ManagerContext managerContext)
        {
            this.managerContext = managerContext;
        }

        public async Task CreateAsync(Manager item)
        {
            await managerContext.CreateAsync(item);
        }

        public async Task<Manager> ReadAsync(int key, bool useNavigationalProperties = false)
        {
            return await managerContext.ReadAsync(key, useNavigationalProperties);
        }

        public async Task<ICollection<Manager>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            return await managerContext.ReadAllAsync(useNavigationalProperties);
        }

        public async Task UpdateAsync(Manager item, bool useNavigationalProperties = false)
        {
            await managerContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await managerContext.DeleteAsync(key);
        }
    }
}
