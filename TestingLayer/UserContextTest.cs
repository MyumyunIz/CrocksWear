using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TestingLayer
{
    public class UserContextTest
    {
        private UserContext context = new UserContext(SetupFixture.dbContext);
        private OrderContext orderContext = new OrderContext(SetupFixture.dbContext);
        private ManagerContext managerContext = new ManagerContext(SetupFixture.dbContext);
        private ShoeContext shoeContext = new ShoeContext(SetupFixture.dbContext);
        private User u1;
        private Order o1;
        private Shoe s1;
        private Manager m1;

        [SetUp]
        public async Task CreateUser()
        {
            u1 = new User("david","davidgrizman@gmail.com");
            await context.CreateAsync(u1);
            m1 = new Manager("myumyun", "myumyun@gmail.com", "0894458934");
            await managerContext.CreateAsync(m1);
            s1 = new Shoe(38,"nike","airmax" , 159,"white","obuvki",new byte[1],m1);
            await shoeContext.CreateAsync(s1);
            o1 = new Order(u1,s1,2,318,158, OrderStatus.InProgress);
            await orderContext.CreateAsync(o1);
            u1.Orders.Add(o1);
            
        }

        [TearDown]
        public void DropUser()
        {
            foreach (User item in SetupFixture.dbContext.Users)
            {
                SetupFixture.dbContext.Users.Remove(item);
            }
            SetupFixture.dbContext.SaveChanges();
        }

        [Test]
        public async Task Create()
        {
            User newUser = new User("david", "davidgrizman@gmail.com");

            int usersBefore = SetupFixture.dbContext.Users.Count();
            await context.CreateAsync(newUser);

            int usersAfter = SetupFixture.dbContext.Users.Count();
            Assert.That(usersBefore + 1 == usersAfter, "Create() does not work!");
        }

        [Test]
        public async Task Read()
        {
            User readUser = await context.ReadAsync(u1.Id,false);

         
            Assert.AreEqual(u1, readUser, "Read does not return the same object!");
            //Assert.That(u1,Is.EqualTo( readUser), "Read does not return the same object!");

        }

        [Test]
        public async Task ReadWithNavigationalProperties()
        {
            User readUser = await context.ReadAsync(u1.Id,true);

            Assert.That(readUser.Orders.Contains(o1), "o1 is not in the orders list!");

        }

        [Test]
        public async Task ReadAll()
        {
            ICollection<User> users = await context.ReadAllAsync() ;

            Assert.That(users.Count != 0, "ReadAll() does not return users!");
        }

        [Test]
        public async Task Delete()
        {
            int usersBefore = SetupFixture.dbContext.Users.Count();

            await context.DeleteAsync(u1.Id);
            int usersAfter = SetupFixture.dbContext.Users.Count();

            Assert.That(usersBefore - 1 == usersAfter, "Delete() does not work! 👎🏻");
        }
        [Test]
        public void TestMethod()
        {
            var answer = 42;
            Assert.That(answer == 42, "Some useful error message");
        }
    }

}

