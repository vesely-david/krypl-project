using DataLayer.Infrastructure;
using DataLayer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Strategy> Strategies { get; set; }
        public virtual ICollection<UserAsset> UserAssets { get; set; }
        public virtual ICollection<ExchangeSecret> ExchangeSecrets { get; set; }
    }
}
