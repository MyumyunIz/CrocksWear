using DataLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingLayer
{
    [SetUpFixture]
    public static class SetupFixture
    {
        public static CrockDBContext dbContext;
        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            // TODO: Add code here that is run before
            //  all tests in the assembly are run
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            dbContext = new CrockDBContext(builder.Options);
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            // TODO: Add code here that is run after
            //  all tests in the assembly have been run
            dbContext.Dispose();
        }

    }
}
