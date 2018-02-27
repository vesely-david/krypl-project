using DataLayer.Infrastructure;
using DataLayer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class User : IdentityUser<int>
    {
        public virtual IEnumerable<Strategy> Strategies { get; set; }
        public virtual IEnumerable<UserAsset> UserAssets { get; set; }
        public virtual IEnumerable<ExchangeSecret> ExchangeSecrets { get; set; }
    }
}
