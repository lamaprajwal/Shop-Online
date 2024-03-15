using Shop.Domain.Models;

namespace Shop.Application.Infrastructure
{
    public partial interface ISessionManager
    {
        string GetId();
        void AddProduct(int stockId,int qty);

        void ClearCart();
        List<CartProduct> GetCart();

        void RemoveProduct(int stockId,int qty);
        void AddCustomerInformation(CustomerInformation customer);

        CustomerInformation GetCustomerInformation();
    }
}
