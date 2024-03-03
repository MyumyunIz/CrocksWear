using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal Price { get; set; }  
        public List<Order> Orders { get; set; }

        public Cart()
        {
            Price = 0;
            Orders = new List<Order>();
        }

    }
}
