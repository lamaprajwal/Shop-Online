using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.ProductsAdmin
{
    public class GetProductsAdmin
    {
        private readonly ApplicationDbContext _ctx;

        public GetProductsAdmin(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public IEnumerable<ProductViewModel> Do()
        {
            return _ctx.Products.ToList().Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Value = x.Value
            });
        }
        public class ProductViewModel
        {
           public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
    }
}
