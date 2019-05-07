using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Models;
using MasterDataManager.Services.Interfaces;
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
        private IMailingService _mailingService;
        private IExchangeSecretRepository _exchangeSecretRepository;
        private IConfiguration _configuration;
        private IMapper _mapper;

        public UserController(
            UserManager<User> userManager,
            IMailingService mailingService,
            IExchangeSecretRepository exchangeSecretRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _mailingService = mailingService;
            _exchangeSecretRepository = exchangeSecretRepository;
            _mapper = mapper;
            _configuration = configuration;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] JsonLoginModel userModel)
        {
            var user = await _userManager.FindByNameAsync(userModel.username);
            if (user != null && await _userManager.CheckPasswordAsync(user, userModel.password))
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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] JsonLoginModel userModel)
        {
            //var user = await _userManager.FindByNameAsync(userModel.username);
            //if (user != null) return BadRequest("User already registered");

            var user = new User
            {
                UserName = userModel.username,
                Email = userModel.email,
            };
            var result = await _userManager.CreateAsync(user, userModel.password);

            //await _mailingService.SendEmailVerification("h.kirchner@seznam.cz", "test");

            if (result.Succeeded) return Ok();
            return BadRequest(result.Errors);
        }

        //[AllowAnonymous]
        //[HttpPost("verify")]
        //public async Task<IActionResult> Verify(string token)
        //{
        //    _mailingService.SendEmailVerification(user.UserName, "test");

        //    return BadRequest("Incorrect combination of username and password");
        //}

        [HttpGet("apikeys")]
        public IActionResult GetApiKeys()
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var keys = _exchangeSecretRepository.GetByUser(userId);
            return Ok(keys.Select(_mapper.Map<JsonExchangeSecretModel>));
        }

        [HttpPost("apikeys")]
        public IActionResult EditApiKeys([FromBody] JsonExchangeSecretModel apiKeyModel)
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var original = _exchangeSecretRepository.GetByUserAndExchange(userId, apiKeyModel.exchangeId);
            if(original == null)
            {
                var secret = _mapper.Map<ExchangeSecret>(apiKeyModel);
                secret.UserId = userId;
                _exchangeSecretRepository.Add(secret);
            }
            else
            {
                original.ApiKey = apiKeyModel.apiKey;
                original.ApiSecret = apiKeyModel.apiSecret;
                _exchangeSecretRepository.Edit(original);
            }
            return Ok();
        }

        [HttpDelete("apikeys/{apiKeyId}")]
        public IActionResult DeleteApiKey(string apiKeyId)
        {
            var userId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return BadRequest("User not found");

            var original = _exchangeSecretRepository.GetById(apiKeyId);
            if (original?.UserId == userId) _exchangeSecretRepository.Delete(original);

            return Ok();
        }
    }
}
