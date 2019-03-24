using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Models
{
    public class ExchangeSecret : IdEntity
    {
        [Required]
        public string ApiKey { get; set; }
        [Required]
        public string ApiSecret { get; set; }
        [Required]
        public string ExchangeId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
