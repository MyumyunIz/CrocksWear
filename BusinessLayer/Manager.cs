using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Manager: User
    {
        

        public List<Shoe> Shoes { get; set; }
        public List<Bill> Bills { get; set; }

        public Manager() {
            Orders = new List<Order> ();
            Bills= new List<Bill>();
            Shoes = new List<Shoe>();
        }

        public Manager(string name, string email, string phone) {
            this.UserName= name;
            this.Email= email;
            this.PhoneNumber= phone;

            Orders = new List<Order>();

            Bills = new List<Bill>();
            Shoes = new List<Shoe>();
        }

    }
}
