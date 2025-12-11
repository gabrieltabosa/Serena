using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Serena.Models;
using Serena.Service;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Serena.Controllers
{
    public class UserController: Controller
    {
        private readonly IUserApiClient _userClient;
        
        public UserController(IUserApiClient userClient)
        {
            _userClient = userClient;
        }

        public async Task<IActionResult> Login(UserViewModel user)
        {
            if (user == null)
            {
                return PartialView("_LoginPartial");

            }
            var resposta = _userClient.AuthenticateAsync(user);
            if ()
            {
                
            }
            return  PartialView("Login successful.");
        }
    }
}
