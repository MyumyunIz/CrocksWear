using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using BusinessLayer;
using ServiceLayer;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace MVCPresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("CrockDBContextConnection") ?? throw new InvalidOperationException("Connection string 'CrockDBContextConnection' not found.");

            builder.Services.AddDbContext<CrockDBContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddRazorPages();
            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<CrockDBContext>();
            builder.Services.AddIdentity<User,IdentityRole>(io =>
            {
                io.Password.RequiredLength = 5;
                io.Password.RequireNonAlphanumeric = false;
                io.Password.RequiredUniqueChars = 0;
                io.Password.RequireUppercase = false;
                io.Password.RequireDigit = false;

                io.User.RequireUniqueEmail = false;

                io.SignIn.RequireConfirmedEmail = false;

                io.Lockout.MaxFailedAccessAttempts = 3;
            }
            )
                .AddEntityFrameworkStores<CrockDBContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddScoped<IEmailSender, EmailSenderManager>();

            builder.Services.AddScoped<IdentityManager, IdentityManager>();
            builder.Services.AddScoped<IdentityContext, IdentityContext>();

            builder.Services.AddScoped<ShoeManager, ShoeManager>();
            builder.Services.AddScoped<ShoeContext, ShoeContext>();

            builder.Services.AddScoped<ManagerManager, ManagerManager>();
            builder.Services.AddScoped<ManagerContext, ManagerContext>();

            builder.Services.AddScoped<OrderManager, OrderManager>();
            builder.Services.AddScoped<OrderContext, OrderContext>();

            builder.Services.AddScoped<TransactionManager, TransactionManager>();
            builder.Services.AddScoped<TransactionContext, TransactionContext>();

            builder.Services.AddScoped<CartManager, CartManager>();
            builder.Services.AddScoped<CartContext, CartContext>();


            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapRazorPages();
            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
