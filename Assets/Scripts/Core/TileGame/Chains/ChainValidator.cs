using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class ChainValidator
{
    protected List<Tile> Tiles;
    protected List<Tile> Selection;


    protected abstract bool moreIsChainFinishable();
    protected abstract bool moreIsChainable(Tile first, Tile next);

    public bool isChainFinishable()
    {
        if (Selection.Count < 3) return false;

        if (Selection.All(o => o.tileType == TileType.Monster)) {
            return false;
        }
        
        return moreIsChainFinishable();
    }

    public bool isEligibleToAddToChain(Tile tile)
    {
        if (Selection.Count == 0)
        {
            return true;
        }

        if (Selection.Contains(tile)) {
            return false;
        }

        Tile lastTile = Selection.ElementAt(Selection.Count - 1);
        if (isChainable(tile, lastTile))
        {
            if (isAdjacent(tile, lastTile)) {
                return true;
            }

            if (hasDirectPath(tile, lastTile)) {
                List<Tile> tilesInBetween = GetInBetweenTiles(tile, lastTile);
                tilesInBetween.Reverse();
                if (areAssumedChainable(tile, tilesInBetween)) {
                    Selection.AddRange(tilesInBetween);
                    return true;
                }
            }
        }

        return false;
    }

    List<Tile> GetInBetweenTiles(Tile first, Tile next) {
        if (!hasDirectPath(first, next)) throw new System.ArgumentException("should never have called this");

        Vector2Int diff = GetTileDiff(first, next);


        Vector2Int normalized = new Vector2Int(System.Math.Sign(diff.x), System.Math.Sign(diff.y));

        Vector2Int stopPoint = new Vector2Int(next.col, next.row);
        Vector2Int pointer = new Vector2Int(first.col, first.row) + normalized;
        List<Tile> betweeners = new List<Tile>();
        int i = 0;
        while (pointer != stopPoint && i < 6) {
            var checkTile = Tiles.FirstOrDefault(o => o.col == pointer.x && o.row == pointer.y);
            betweeners.Add(checkTile);
            pointer += normalized;
            i += 1; // because i'm scared
        }

        return betweeners;
    }

    bool hasDirectPath(Tile first, Tile next) {
        Vector2Int diff = GetTileDiff(first, next);

        if (diff.x == 0 || diff.y == 0) return true;
        if (Mathf.Abs(diff.x) == Mathf.Abs(diff.y)) return true;

        return false;
    }


    bool areAssumedChainable(Tile first, List<Tile> betweeners) {
        return betweeners.All(o => isChainable(first, o) && !Selection.Contains(o));
    }

    bool isChainable(Tile first, Tile next)
    {
        TileType[] attackChainTiles = new TileType[] { TileType.Sword, TileType.Monster };
        TileType[] goldChainTiles = new TileType[] { TileType.Coin, TileType.Treasure };
        if (first.tileType == next.tileType) return true;

        if (
            attackChainTiles.Contains(first.tileType)
            && attackChainTiles.Contains(next.tileType)
            && Selection.All((o) => attackChainTiles.Contains(o.tileType))
        ) {
            return true;
        }

        if (
            goldChainTiles.Contains(first.tileType)
            && goldChainTiles.Contains(next.tileType)
            && Selection.All((o) => goldChainTiles.Contains(o.tileType))
        ) {
            return true;
        }

        return moreIsChainable(first, next);
    }

    bool isAdjacent(Tile tile1, Tile tile2)
    {
        Vector2Int diff = GetAbsTileDiff(tile1, tile2);
        return (diff.y == 1 || diff.x == 1) && diff.x + diff.y <= 2;
    }

    Vector2Int GetAbsTileDiff(Tile first, Tile next) {
        return new Vector2Int(Mathf.Abs(next.col - first.col), Mathf.Abs(next.row - first.row));
    }

    Vector2Int GetTileDiff(Tile first, Tile next) {
        return new Vector2Int(next.col - first.col, next.row - first.row);
    }

    public bool doesTileMatchLastInSelection(Tile tile)
    {
        if (Selection.Count == 0) return false;

        var lastTile = Selection.ElementAt(Selection.Count - 1);

        return lastTile == tile;
    }
}
