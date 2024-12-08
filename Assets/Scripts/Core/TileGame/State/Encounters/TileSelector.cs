using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileSelector
{
    List<TileType> Options;

    public TileSelector(List<TileType> options) {
        Options = options;
    }

    public TileType GetNextTile(ITileCollectorContext context)
    {
        if (context == null) {
            return GetRandomTile();
        }
        int tilesCleared = context.TilesCleared;

        if (tilesCleared > 4 && tilesCleared % 8 == 0)
        {
            return TileType.Monster;
        }

        return GetRandomTile();
    }

    TileType GetRandomTile()
    {
        int tileChoice = Random.Range(0, Options.Count);
        return Options.ElementAt(tileChoice);
    }
}

// TILESCLEARED = context.GetTilesCleared();
// But i don't think context is updated between
// tiles cleane dup