using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RethoughtLib.Classes.FeatureV2
{
    using global::RethoughtLib.Classes.FeatureV2.Abstract_Classes;

    public sealed class Parent : ParentBase
    {
        public Parent(string name)
        {
            this.Name = name;
        }

        /// <summary>Called when [load].</summary>
        protected override void OnLoad()
        {
            base.OnLoad();
        }
    }
}
