using System.Collections.Generic;
using System.Linq;

public class StandardChainValidator : ChainValidator
{
    public StandardChainValidator(ITileGridProvider tgp, List<Tile> selection) {
        Tiles = tgp.GetTiles().TileList;
        Selection = selection;
    }
    protected override bool moreIsChainFinishable() {
        return true;
    }
    protected override bool moreIsChainable(Tile first, Tile next) {
        return false;
    }
}
