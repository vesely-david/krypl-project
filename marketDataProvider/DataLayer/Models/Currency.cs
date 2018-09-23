using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Currency : IdEntity
    {
        public string Name { get; set; }
    }
}
