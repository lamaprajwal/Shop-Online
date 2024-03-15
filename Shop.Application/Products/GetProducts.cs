using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    public class GetProducts
    {
        private readonly ApplicationDbContext _ctx;

        public GetProducts(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public IEnumerable<ProductViewModel> Do()
        {
            return _ctx.Products
                .Include(x=>x.Stock)
                .Select(x => new ProductViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Value = $"Rs.{x.Value.ToString("N2")}",
                OutOfStock=(x.Stock.Sum(y=>y.Qty) == 0 ),
                Id = x.Id,
               
            }).ToList();
        }
        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }

            public int Id {  get; set; }
            public int StockCount  {  get; set; }

            public bool OutOfStock {  get; set; }
        }
    }
}
