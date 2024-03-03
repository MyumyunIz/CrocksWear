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
            Manager m = new Manager();

            Shoe s = new Shoe(40, "nike", "airmax", 100, "white", "descriptiontext", new byte[2],m);
            await shoeManager.CreateAsync(s);

            Console.WriteLine(s.Id);
            
        }
    }
}
