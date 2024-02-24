using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public byte[] Image_bytes { get; set; }
        public Image() 
        {
        
        }
        public Image(byte[] image_bytes)
        {
            Image_bytes = image_bytes;
        }
    }
}
