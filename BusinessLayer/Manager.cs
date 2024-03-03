using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Manager
    {
        [Key]
        public int Id { get; set; }
        //[Required]
        //public User User { get; set; }
        public List<Shoe> Shoes { get; set; }
        public List<Transaction> Bills { get; set; }

        public Manager() {
            
            Bills= new List<Transaction>();
            Shoes = new List<Shoe>();
        }

        //public Manager(User user)
        //{

        //    Bills = new List<Bill>();
        //    Shoes = new List<Shoe>();
        //}

    }
}
