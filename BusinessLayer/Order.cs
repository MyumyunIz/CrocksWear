using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Order
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        
        [Required]
        public Shoe Shoe { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Range(0,999999)]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        [Range(0, 999999)]
        [Required]
        [Precision(18, 2)]
        public decimal Shoeprice { get; set; }
        public OrderStatus Status { get; set; }
        //[AllowNull]
        public Transaction? Transaction { get; set; }


        public Order    ()
        {
            //Bill = new Bill ();
        }



        public Order( Shoe shoe, int quantity, decimal price, decimal shoeprice, OrderStatus status)
        {
            this.Shoe= shoe;
            this.Quantity= quantity;
            this.Price= price;
            this.Shoeprice= shoeprice;
            this.Status= status;

            //Bill = new Bill();

        }
















    }
}
