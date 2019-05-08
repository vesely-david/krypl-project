using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataLayer.Models;
using DataLayer.Infrastructure.Interfaces;
using MasterDataManager.Models;
using Microsoft.AspNetCore.Identity;
using DataLayer.Enums;

namespace MasterDataManager.Controllers
{
    [Produces("application/json")]
    [Route("admin")]
    [Authorize]
    public class AdminController : Controller
    {
        private UserManager<User> _userManager;
        private IExchangeSecretRepository _exchangeSecretRepository;

        public AdminController(
            UserManager<User> userManager,
            IExchangeSecretRepository exchangeSecretRepository)
        {
            _userManager = userManager;
            _exchangeSecretRepository = exchangeSecretRepository;
        }
        //================ SEED METHODS =====================

        [AllowAnonymous]
        [HttpPost("seedEverything")]
        public async Task<StatusCodeResult> SeedEverything()
        {
            var users = SeedUsersAsync();
            var exchangeSecrets = SeedExchangeSecrets("kirchjan");

            await users;
            return Ok();
        }


        [AllowAnonymous]
        [Route("seedUsers")]
        [HttpPost]
        public async Task<StatusCodeResult> SeedUsersAsync()
        {
            var user1 = await _userManager.FindByNameAsync("veselda7");
            if (user1 == null)
            {
                var david = new User
                {
                    Email = "veselda7@gmail.com",
                    EmailConfirmed = true,
                    UserName = "veselda7"
                };
                var a = await _userManager.CreateAsync(david, "veselda7!Hesl0");
            }
            var user2 = await _userManager.FindByNameAsync("kirchjan");
            if (user2 == null)
            {
                var honza = new User
                {
                    Email = "h.kirchner@seznam.cz",
                    EmailConfirmed = true,
                    UserName = "kirchjan",

                };
                var a = await _userManager.CreateAsync(honza, "kirchjan!Hesl0");

                honza.Strategies = new List<Strategy>
                    {
                        new Strategy
                        {
                            Start = DateTime.Now,
                            TradingMode = TradingMode.Real,
                            IsOverview = true,
                            Evaluations = new List<EvaluationTick>
                            {
                                new EvaluationTick
                                {
                                    TimeStamp = DateTime.Now,
                                    BtcValue = 0,
                                    UsdValue = 0
                                }
                            }

                        },
                        new Strategy
                        {
                            Start = DateTime.Now,
                            TradingMode = TradingMode.PaperTesting,
                            IsOverview = true,
                            Evaluations = new List<EvaluationTick>
                            {
                                new EvaluationTick
                                {
                                    TimeStamp = DateTime.Now,
                                    BtcValue = 0,
                                    UsdValue = 0
                                }
                            }
                        },
                        new Strategy
                        {
                            Start = DateTime.Now,
                            TradingMode = TradingMode.BackTesting,
                            IsOverview = true,
                            Evaluations = new List<EvaluationTick>
                            {
                                new EvaluationTick
                                {
                                    TimeStamp = DateTime.Now,
                                    BtcValue = 0,
                                    UsdValue = 0
                                }
                            }
                        }
                    };
                var r  = await _userManager.UpdateAsync(honza);
            }

            return Ok();
        }

        [HttpPost("seedUserSecrets")]
        public async Task<StatusCodeResult> SeedExchangeSecrets(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            _exchangeSecretRepository.Add(new ExchangeSecret
            {
                ApiKey = "vmPUZE6mv9SD5VNHk4HlWFsOr6aKE2zvsw0MuIgwCIPy6utIco14y7Ju91duEh8A",
                ApiSecret = "NhqPtmdSJYdKjVHjA7PZj4Mge3R5YNiP1e3UZjInClVN65XAbvqqM6A7H5fATj0j",
                ExchangeId = "binance",
                UserId = user.Id
            });


            return Ok();
        }
    }
}
