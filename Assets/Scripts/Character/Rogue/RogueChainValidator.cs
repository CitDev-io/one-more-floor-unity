using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace citdev{
    public class RogueChainValidator : ChainValidator
    {
        public RogueChainValidator(List<GameTile> tiles, List<GameTile> selection) {
            Tiles = tiles;
            Selection = selection;
        }
        protected override bool moreIsSelectionFinishable() {
            return true;
        }
        protected override bool moreIsChainable(GameTile first, GameTile next) {

            return false;
        }
    }
}
