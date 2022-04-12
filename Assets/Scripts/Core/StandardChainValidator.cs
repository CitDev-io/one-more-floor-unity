using System.Collections.Generic;
using System.Linq;

public class StandardChainValidator : ChainValidator
{
    public StandardChainValidator(List<Tile> tiles, List<Tile> selection) {
        Tiles = tiles;
        Selection = selection;
    }
    protected override bool moreIsSelectionFinishable() {
        return true;
    }
    protected override bool moreIsChainable(Tile first, Tile next) {
        return false;
    }
}
