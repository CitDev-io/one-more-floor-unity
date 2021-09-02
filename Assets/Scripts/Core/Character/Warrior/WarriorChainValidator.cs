using System.Collections.Generic;
using System.Linq;

public class WarriorChainValidator : ChainValidator
{
    public WarriorChainValidator(List<Tile> tiles, List<Tile> selection) {
        Tiles = tiles;
        Selection = selection;
    }
    protected override bool moreIsSelectionFinishable() {
        return true;
    }
    protected override bool moreIsChainable(Tile first, Tile next) {
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
