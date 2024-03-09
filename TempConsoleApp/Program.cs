using BusinessLayer;
using DataLayer;
using ServiceLayer;

namespace TempConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var dbContext = new CrockDBContext();
            var shoeContext = new ShoeContext(dbContext);
            var shoeManager = new ShoeManager(shoeContext);

            Shoe s = await shoeManager.ReadAsync(4, true);
            

        }
    }
}
