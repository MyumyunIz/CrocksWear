using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class User : IdentityUser
    {

        public Cart Cart { get; set; }
        public Manager Manager { get; set; }
        public List<Order> Orders { get; set; }
        

        public User() {
            Orders = new List<Order>();
            Manager = new Manager();
            Cart = new Cart();

        }

        public User(string username, string email) 
        {
            this.UserName= username;
            this.Email= email;

            Manager = new Manager();
            Orders = new List<Order>();
            Cart = new Cart();
        }

        public override string ToString()
        {
            return string.Format($"{Id} {UserName} {Email}");
        }

        public static explicit operator User(ValueTask<IdentityUser> v)
        {
            throw new NotImplementedException();
        }
    }
}
