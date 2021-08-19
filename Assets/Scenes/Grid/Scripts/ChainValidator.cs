using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace citdev {
public class ChainValidator
{
    List<GameTile> Tiles;
    List<GameTile> Selection;
    public ChainValidator(List<GameTile> tiles, List<GameTile> selection) {
        Tiles = tiles;
        Selection = selection;
    }

    public bool isSelectionFinishable()
    {
        if (Selection.Count < 3) return false;

        if (Selection.Any(o => o.tileType == TileType.Monster)
        && !Selection.Any(o => o.tileType == TileType.Sword)) {
            return false;
        }
        
        return true;
    }

    public bool isEligibleToAddToSelection(GameTile tile)
        {
            if (Selection.Count == 0)
            {
                return true;
            }

            if (Selection.Contains(tile)) {
                return false;
            }

            GameTile lastTile = Selection.ElementAt(Selection.Count - 1);
            if (isChainable(tile, lastTile))
            {
                if (isAdjacent(tile, lastTile)) {
                    return true;
                }

                if (hasDirectPath(tile, lastTile)) {
                    List<GameTile> tilesInBetween = GetInBetweenTiles(tile, lastTile);
                    tilesInBetween.Reverse();
                    if (areAssumedChainable(tile, tilesInBetween)) {
                        Selection.AddRange(tilesInBetween);
                        return true;
                    }
                }
            }

            return false;
        }

        List<GameTile> GetInBetweenTiles(GameTile first, GameTile next) {
            if (!hasDirectPath(first, next)) throw new System.ArgumentException("should never have called this");
    
            Vector2Int diff = GetTileDiff(first, next);


            Vector2Int normalized = new Vector2Int(System.Math.Sign(diff.x), System.Math.Sign(diff.y));

            Vector2Int stopPoint = new Vector2Int(next.col, next.row);
            Vector2Int pointer = new Vector2Int(first.col, first.row) + normalized;
            List<GameTile> betweeners = new List<GameTile>();
            int i = 0;
            while (pointer != stopPoint && i < 6) {
                var checkTile = Tiles.FirstOrDefault(o => o.col == pointer.x && o.row == pointer.y);
                betweeners.Add(checkTile);
                pointer += normalized;
                i += 1; // because i'm scared
            }

            return betweeners;
        }

        bool hasDirectPath(GameTile first, GameTile next) {
            Vector2Int diff = GetTileDiff(first, next);

            if (diff.x == 0 || diff.y == 0) return true;
            if (Mathf.Abs(diff.x) == Mathf.Abs(diff.y)) return true;

            return false;
        }


        bool areAssumedChainable(GameTile first, List<GameTile> betweeners) {
            return betweeners.All(o => isChainable(first, o) && !Selection.Contains(o));
        }

        bool isChainable(GameTile first, GameTile next)
        {
            TileType[] attackChainTiles = new TileType[] { TileType.Sword, TileType.Monster };
            if (first.tileType == next.tileType) return true;

            if (
                attackChainTiles.Contains(first.tileType)
                && attackChainTiles.Contains(next.tileType)
            ) {
                return true;
            }

            return false;
        }

        bool isAdjacent(GameTile tile1, GameTile tile2)
        {
            Vector2Int diff = GetAbsTileDiff(tile1, tile2);
            return (diff.y == 1 || diff.x == 1) && diff.x + diff.y <= 2;
        }

        Vector2Int GetAbsTileDiff(GameTile first, GameTile next) {
            return new Vector2Int(Mathf.Abs(next.col - first.col), Mathf.Abs(next.row - first.row));
        }

        Vector2Int GetTileDiff(GameTile first, GameTile next) {
            return new Vector2Int(next.col - first.col, next.row - first.row);
        }

        public bool doesTileMatchLastInSelection(GameTile tile)
        {
            if (Selection.Count == 0) return false;

            var lastTile = Selection.ElementAt(Selection.Count - 1);

            return lastTile == tile;
        }
}
}