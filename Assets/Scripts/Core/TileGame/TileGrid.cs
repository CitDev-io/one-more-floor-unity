using System.Collections.Generic;
using System.Linq;

public class TileGrid {
    public List<Tile> TileList = new();
    Encounter _encounter;

    public TileGrid(Encounter encounter) {
        _encounter = encounter;
        GenerateGrid();
    }

    void GenerateGrid() {
        TileList.Clear();
        for (var rowid = 0; rowid < _encounter.rows; rowid++) {
            for (var colid = 0; colid < _encounter.cols; colid++) {
                Tile t = new Tile(
                    colid,
                    rowid,
                    _encounter.GetNextMonster(),
                    _encounter.GetNextTile()
                );
                TileList.Add(t);
            }
        }
    }

    public void FlushCollectedAndDeadTiles() {
        var tilesToRepop = TileList.FindAll((o) => o.IsBeingCollected || !o.isAlive()).OrderByDescending(o => o.row);
        foreach(Tile t in tilesToRepop) {
            PickupReskinAndDropTile(t);
        }
    }

    public void TagCollectibleTiles(List<Tile> tiles) {
        var collectibleTiles = tiles.Where((o) => o.tileType != TileType.Monster || !o.isAlive()).ToList();

        foreach(Tile t in collectibleTiles) {
            t.IsBeingCollected = true;
        }
    }
            

    void PickupReskinAndDropTile(Tile tile)
    {
        // Current logic:
        // called on tiles that are being collected only!!
        // get all things above me
        // call dropdown on the tile (lower your row by 1)
        // replace with a random new tile
        // put it on top and go to y=5
        
        List<Tile> aboveTiles = TileList.FindAll((o) => o.col == tile.col && o.row > tile.row);

        foreach(Tile t in aboveTiles)
        {
            t.Dropdown();
        }

        TileType newTileType = _encounter.GetNextTile();
        tile.CurrentMonster = _encounter.GetNextMonster();

        tile.ClearAndDropTileAs(newTileType);
    }

}
