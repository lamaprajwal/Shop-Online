using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.ProductsAdmin
{
    public class UpdateProduct
    {
        private ApplicationDbContext _context;

        public UpdateProduct(ApplicationDbContext context)
        {
            _context = context;
        }
        
        
        public async Task<Response> Do(Request request)
        {
            try
            {
                var product = _context.Products.Where(x => x.Id == request.Id).FirstOrDefault();

                product.Name = request.Name;
                product.Description = request.Description;
                product.Value = request.Value;


                await _context.SaveChangesAsync();
                return new Response
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Value = product.Value
                };
            }
            catch (Exception ex)
            {
                // Handle the exception here, you can log it or throw a custom exception.
                // For example, you can log the exception and then throw a custom exception.
                // Logging should be done using a proper logging library like Serilog or NLog.

                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
               

                
                // You can re-throw a custom exception if needed.
                throw new ApplicationException("An error occurred while updating the product.");
            }
        }



        [Serializable]
        public class Request
        {
            public Request()
            {
                
            }
            public int Id {  get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
        [Serializable]
        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }

        }
    }
}

