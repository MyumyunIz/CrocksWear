using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public static class ContextGenerator
    {
        public static CrockDBContext dbContext;
        public static UserContext usersContext;
        public static OrderContext ordersContext;
        public static ShoeContext shoesContext;
        public static BillContext billsContext;
        public static ManagerContext managersContext;

        public static CrockDBContext GetDbContext()
        {
            if (dbContext == null)
            {
                SetDbContext();
            }
            return dbContext;
        }

        public static void SetDbContext()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }

            dbContext = new CrockDBContext();
        }

        public static UserContext GetUsersContext()
        {
            if (usersContext == null)
            {
                SetUsersContext();
            }

            return usersContext;
        }

        public static void SetUsersContext()
        {
            usersContext = new UserContext(GetDbContext());
        }

        public static OrderContext GetOrdersContext()
        {
            if (ordersContext == null)
            {
                SetOrdersContext();
            }

            return ordersContext;
        }

        public static void SetOrdersContext()
        {
            ordersContext = new OrderContext(GetDbContext());
        }

        public static ShoeContext GetShoesContext()
        {
            if (shoesContext == null)
            {
                SetShoesContext();
            }

            return shoesContext;
        }

        public static void SetShoesContext()
        {
            shoesContext = new ShoeContext(GetDbContext());
        }

        public static BillContext GetBillsContext()
        {
            if (billsContext == null)
            {
                SetBillsContext();
            }

            return billsContext;
        }

        public static void SetBillsContext()
        {
            billsContext = new BillContext(GetDbContext());
        }

        public static ManagerContext GetManagersContext()
        {
            if (managersContext == null)
            {
                SetManagersContext();
            }

            return managersContext;
        }

        public static void SetManagersContext()
        {
            managersContext = new ManagerContext(GetDbContext());
        }

    }
}
