using Microsoft.AspNetCore.Http;

using Shop.Application.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Cart
{
    public class GetCustomerInformation
    {
        private ISessionManager _sessionManager;
        public GetCustomerInformation(ISessionManager session)
        {
            _sessionManager = session;
        }
        public class Request
        {
           
            public string FirstName { get; set; }
            
            public string LastName { get; set; }
           
            public string Email { get; set; }
            
            public string PhoneNumber { get; set; }
            
            public string Address1 { get; set; }
            
            public string Address2 { get; set; }
            
            public string City { get; set; }
            
            public string PostCode { get; set; }
        }
        public Request Do()
        {
            var customerInformation=_sessionManager.GetCustomerInformation();
        
            if(customerInformation==null)
                return null;

            return new Request
            {
                FirstName = customerInformation.FirstName,
                LastName = customerInformation.LastName,
                Email = customerInformation.Email,
                PhoneNumber = customerInformation.PhoneNumber,
                Address1 = customerInformation.Address1,
                Address2 = customerInformation.Address2,
                City = customerInformation.City,
                PostCode = customerInformation.PostCode,

            };

        }
    }
}
