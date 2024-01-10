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
        private User u1;
        private Order o1;
        private Shoe s1;
        private Manager m1;

        [SetUp]
        public async Task CreateUser()
        {
            u1 = new User(10,"david","davidgrizman@gmail.com","123123123");
            await context.CreateAsync(u1);
            m1 = new Manager(10, "myumyun", "myumyun@gmail.com", "12313234", "0894458934");
            s1 = new Shoe(10,38,"nike","airmax" , 159,"white","obuvki",m1);
            o1 = new Order(10,u1,s1,2,318,158, OrderStatus.InProgress);
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
            User newUser = new User(14, "david", "davidgrizman@gmail.com", "123123123");

            int usersBefore = SetupFixture.dbContext.Users.Count();
            await context.CreateAsync(newUser);

            int usersAfter = SetupFixture.dbContext.Users.Count();
            Assert.That(usersBefore + 1 == usersAfter, "Create() does not work!");
        }

        [Test]
        public async Task Read()
        {
            User readUser = await context.ReadAsync(u1.Id);

         
            Assert.That(u1,Is.EqualTo( readUser), "Read does not return the same object!");

        }

        [Test]
        public async Task ReadWithNavigationalProperties()
        {
            User readUser = await context.ReadAsync(u1.Id,true);

            Assert.That(readUser.Orders.Contains(o1), "o1 is not in the bottles list!");

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

