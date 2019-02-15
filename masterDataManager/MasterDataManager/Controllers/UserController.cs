using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using MasterDataManager.Models;
using MasterDataManager.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MasterDataManager.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private UserManager<User> _userManager;
        private IConfiguration _configuration;

        public UserController(
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] JsonLoginModel userModel)
        {
            var username = userModel.username;
            var password = userModel.password;
            var user = await _userManager.FindByNameAsync(username);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                var securityKey = _configuration["JwtTokens:Secret"];
                var token = JwtTokenUtils.GenerateToken(user, securityKey);
                var validUntil = token.Claims.FirstOrDefault(o => o.Type == "exp")?.Value;
                return Json(new
                {
                    jwt = new JwtSecurityTokenHandler().WriteToken(token),
                    validUntil
                });
            }
            return BadRequest("Incorrect combination of username and password");
        }
    }
}
