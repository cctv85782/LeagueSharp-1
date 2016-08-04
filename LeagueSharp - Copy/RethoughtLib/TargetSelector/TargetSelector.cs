using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RethoughtLib.TargetSelector
{
    using global::RethoughtLib.TargetSelector.Abstract_Classes;

    using LeagueSharp;
    using LeagueSharp.Common;

    public class TargetSelector : TargetSelectorBase
    {
        public Obj_AI_Hero LastSelectedTarget { get; set; }

        public Obj_AI_Hero LastManuallySelectedTarget { get; set; }

        public Obj_AI_Hero SelectedTarget { get; set; }

        public Obj_AI_Hero ManuallySelectedTarget { get; set; }

        public TargetSelector(Menu menu)
            : base(menu)
        {
        }
    }
}
