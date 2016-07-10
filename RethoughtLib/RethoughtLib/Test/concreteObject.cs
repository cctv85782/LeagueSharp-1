using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RethoughtLib.Test
{
    internal class ConcreteObject : AbstractObject
    {
        protected override string SomeString { get; set; } = "X";
    }
}
