using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RethoughtLib.Classes.FeatureV2.Implementations
{
    using global::RethoughtLib.Classes.FeatureV2.Abstract_Classes;

    public sealed class RootParent : ParentBase
    {
        public RootParent(string name)
        {
            this.Name = name;
        }


    }
}
