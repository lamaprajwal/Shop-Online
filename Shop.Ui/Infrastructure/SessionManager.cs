using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shop.Application.Infrastructure;
using Shop.Domain.Models;
using static Shop.Application.Infrastructure.ISessionManager;


namespace Shop.Ui.Infrastructure
{
   

        public class SessionManager : ISessionManager
        {
            private readonly ISession _session;

        private const string Cart = "cart";
        private const string CustomerInfo = "customer-info";
            public SessionManager(IHttpContextAccessor httpContextAccessor)
            {
            _session = httpContextAccessor.HttpContext.Session;
            }

        public void AddCustomerInformation(CustomerInformation customer)
        {
            var stringObject = JsonConvert.SerializeObject(customer);
            _session.SetString(CustomerInfo, stringObject);
        }

        public void AddCustomerInformation()
        {
            throw new NotImplementedException();
        }

        public void AddProduct(int stockId, int qty)
            {
                
                var cartList = new List<CartProduct>();
                var stringObject = _session.GetString(Cart);
                if (!string.IsNullOrEmpty(stringObject))
                {
                    cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
                }
                if (cartList.Any(x => x.StockId == stockId))
                {
                    cartList.Find(x => x.StockId == stockId).Qty += qty;
                }
                else
                {
                    cartList.Add(new CartProduct
                    {
                        StockId = stockId,
                        Qty = qty,
                    });
                }

                stringObject = JsonConvert.SerializeObject(cartList);

                _session.SetString(Cart, stringObject);
            }

        public void ClearCart()
        {
           _session.Remove(Cart);
        }

        public List<CartProduct> GetCart()
            {
            var stringObject = _session.GetString(Cart);
            if (string.IsNullOrEmpty(stringObject))
                return null;

            var cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
            return cartList;
        }

        public CustomerInformation GetCustomerInformation()
        {
            var stringObject = _session.GetString(CustomerInfo);

            if (String.IsNullOrEmpty(stringObject))
                return null;

            var customerInformation = JsonConvert.DeserializeObject<Shop.Domain.Models.CustomerInformation>(stringObject);
            return customerInformation;
        }

        public string GetId()=>_session.Id;

        public void RemoveProduct(int stockId, int qty)
        {
            var cartList = new List<CartProduct>();
            var stringObject = _session.GetString(Cart);

            if (string.IsNullOrEmpty(stringObject)) return;
            
cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
           
            if (!cartList.Any(x => x.StockId ==stockId)) return;


            var product =cartList.First(x=> x.StockId ==stockId);
            product.Qty -= qty;

            if(product.Qty <=0)
            {
                cartList.Remove(product);
            }
           
            
           
            stringObject = JsonConvert.SerializeObject(cartList);
            _session.SetString(Cart, stringObject);
        }
    }
}
