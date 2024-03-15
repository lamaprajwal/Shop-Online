using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string FileName { get; set; }
        // Other image properties

        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
