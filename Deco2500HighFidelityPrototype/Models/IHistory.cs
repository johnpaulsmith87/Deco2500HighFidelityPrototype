using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public interface IHistory
    {
        Guid UserId { get; set; }
        DateTime EventDateTime { get; set; }
    }
}
