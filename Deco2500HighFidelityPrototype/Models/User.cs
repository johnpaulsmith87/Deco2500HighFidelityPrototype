using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public class User
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public Preferences Preferences {get;set;}
        public DateTime DOB { get; set; }
    }
}
