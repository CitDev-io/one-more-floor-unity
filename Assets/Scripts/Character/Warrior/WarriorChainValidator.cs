using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace citdev{
    public class WarriorChainValidator : ChainValidator
    {
        public WarriorChainValidator(List<GameTile> tiles, List<GameTile> selection) {
            Tiles = tiles;
            Selection = selection;
        }
        protected override bool moreIsSelectionFinishable() {
            return true;
        }
        protected override bool moreIsChainable(GameTile first, GameTile next) {
            TileType[] shieldBashChainTiles = new TileType[] { TileType.Shield, TileType.Monster };

            if (
                shieldBashChainTiles.Contains(first.tileType)
                && shieldBashChainTiles.Contains(next.tileType)
                && Selection.All((o) => shieldBashChainTiles.Contains(o.tileType))
            ) {
                return true;
            }
            return false;
        }
    }
}
