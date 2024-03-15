using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models
{

    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }

        public Image image { get; set; }

        public Stripe Stripe { get; set; }
        public ICollection<Stock> Stock { get; set; }
        public ICollection<OrderStock> OrderProducts { get; set; }
    }
}
