using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deco2500HighFidelityPrototype.Models
{
    public class Preferences
    {
        // put a bunch of bools?
        public bool Privacy { get; set; }

        public static Preferences GetDefaultPreferences() => new Preferences() { Privacy = false };
    }
}
