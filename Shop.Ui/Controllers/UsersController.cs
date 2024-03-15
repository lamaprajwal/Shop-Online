﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.ProductsAdmin;
using Shop.Application.StockAdmin;
using Shop.Application.UsersAdmin;
using Shop.Database;

namespace Shop.Ui.Controllers
{

    [Route("[controller]")]
    
    [Authorize(Policy = "Admin")]
    public class UsersController : Controller
    {
        private CreateUser _createUser;

        public UsersController(CreateUser createUser)
        {
            _createUser=createUser;
           
        }

        public async Task<IActionResult> CreateUser([FromBody] CreateUser.Request request)
        {
          await  _createUser.Do(request);
            return Ok(request);
        }


    }
}
