using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rethought_Yasuo.Yasuo.Modules.GraphGenerator
{
    using RethoughtLib.Algorithm.Pathfinding;

    using SharpDX;

    internal class Node : NodeBase
    {
        public Node(Vector3 position)
            : base(position)
        {
        }
    }
}
