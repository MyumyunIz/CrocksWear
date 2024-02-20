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
        
        
        
        public List<Order> Orders { get; set; }
        

        public User() {
            Orders = new List<Order>();
        }

        public User(string username, string email) 
        {
            this.UserName= username;
            this.Email= email;


            Orders = new List<Order>();

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
