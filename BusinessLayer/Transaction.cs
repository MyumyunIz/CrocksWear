﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Transaction
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string BankCard { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        [Required]
        public List<Order> Orders { get; set; }
            


        public Transaction() { 
            Orders = new List<Order>();
        }

        public Transaction(User user, string address, string bankcard, decimal price, List<Order> orders) {
            this.User = user;
            this.Address = address;
            this.BankCard = bankcard;
            this.Price = price;
            Orders = orders;

        }


    }
}
