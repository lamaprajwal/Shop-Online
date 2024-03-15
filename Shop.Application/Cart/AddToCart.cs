using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.Internal;

using Shop.Application.Infrastructure;
using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Shop.Application.Cart
{
    public class AddToCart
    {
        private ISessionManager _session;
        private ApplicationDbContext _ctx;

        public AddToCart(ISessionManager sessionManager,ApplicationDbContext ctx)
        {

          _session = sessionManager;
            _ctx=ctx;
        }
        public class Request
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }
        public async Task<bool>  Do(Request request)
        {
            var stockOnHold=_ctx.StockOnHolds.Where(x=>x.SessionId==_session.GetId()).ToList();
            var stockToHold=_ctx.Stock.Where(x=>x.Id == request.StockId).FirstOrDefault();
            if(stockToHold.Qty<request.Qty)
            {
                return false;
            }
            if(stockOnHold.Any(x=>x.StockId==request.StockId))
            {
                stockOnHold.Find(x => x.StockId == request.StockId).Qty += request.Qty;
            }
            else
            {

            _ctx.StockOnHolds.Add(new StockOnHold
            {
                StockId = stockToHold.Id,
                SessionId = _session.GetId(),
                Qty = request.Qty,
                ExpiryDate = DateTime.Now.AddMinutes(20),

            }) ;
            }
            stockToHold.Qty = stockToHold.Qty - request.Qty;
            foreach(var stock in stockOnHold)
            {
                stock.ExpiryDate = DateTime.Now.AddMinutes(20);
            }
            await _ctx.SaveChangesAsync();

            _session.AddProduct(request.StockId,request.Qty);

            
            return true;
        }
    }
}
