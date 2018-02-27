using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Currency : IdEntity, IEqualityComparer<Currency>
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public bool Equals(Currency x, Currency y)
        {
            return (x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase) || 
                x.Code.Equals(y.Code, StringComparison.OrdinalIgnoreCase));
        }

        public int GetHashCode(Currency obj)
        {
            return obj.GetHashCode();
        }
    }
}
