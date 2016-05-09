using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp.Common;
using LeagueSharp;

using ItemData = LeagueSharp.Common.Data.ItemData;

namespace Yasuo.Common.LogicProvider
{
    class ItemLogicProvider
    {
        public Items.Item Tiamat, Hydra, Shiv, InfinityEdge, TrinityForce, Sheen;

        public ItemLogicProvider()
        {
            this.SetItems();
        }

        private void SetItems()
        {
            Tiamat = ItemData.Tiamat_Melee_Only.GetItem();
            Hydra = ItemData.Ravenous_Hydra_Melee_Only.GetItem();
            Sheen = ItemData.Sheen.GetItem();
            TrinityForce = ItemData.Trinity_Force.GetItem();
            Shiv = ItemData.Statikk_Shiv.GetItem();
            InfinityEdge = ItemData.Infinity_Edge.GetItem();
        }

        public void HasItem(ItemData itemData)
        {
            
        }
    }
}
