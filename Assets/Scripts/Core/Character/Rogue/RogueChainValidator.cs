using System.Collections.Generic;

public class RogueChainValidator : ChainValidator
{
    public RogueChainValidator(List<Tile> tiles, List<Tile> selection) {
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
