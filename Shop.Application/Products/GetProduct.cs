using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    public class GetProduct
    {
        private readonly ApplicationDbContext _ctx;

        public GetProduct(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task< ProductViewModel> Do(string name)
        {
            var stocksOnHold = _ctx.StockOnHolds.Where(x => x.ExpiryDate < DateTime.Now).ToList();
            if(stocksOnHold.Count  > 0)
            {
                var stockToReturn=_ctx.Stock.AsEnumerable().Where(x=>stocksOnHold.Any(y=>y.StockId==x.Id)).ToList();
                foreach(var stock in stockToReturn)
                {
                    stock.Qty = stock.Qty + stocksOnHold.FirstOrDefault(x => x.StockId == stock.Id).Qty;
                }

                _ctx.StockOnHolds.RemoveRange(stocksOnHold);
                await _ctx.SaveChangesAsync();
            }

          return  _ctx.Products
            .Include(x => x.Stock)
            .Where(x => x.Name == name)
            .Select(x => new ProductViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Value = $"Rs.{x.Value.ToString("N2")}",
                id=x.Id,

                Stock = x.Stock.Select(
                    y => new StockViewModel
                    {
                        id = y.Id,
                        Description = y.Description,
                        Qty=y.Qty,
                    })
            })
            .FirstOrDefault();
        }
        
        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }

            public int id;
            public IEnumerable<StockViewModel> Stock { get; set; }
        }
        public class StockViewModel
        {
            public int id { get; set; }
           
            public string Description { get; set; }
            public int Qty { get; set; }
        }
       
    }
}
