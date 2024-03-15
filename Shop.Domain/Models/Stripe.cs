using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models
{
    public class Stripe
    {
        public int StripeId { get; set; }
        public string TestId { get; set; } // Stripe test ID property
                                           // Other properties...

        public int ProductId { get; set; } // Foreign key
        public Product Product { get; set; }

    }
}
