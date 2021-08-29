using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace citdev {
    public class TileSelector
    {
        List<TileType> Options;
        int tilesCleared = 0;

        public TileSelector(List<TileType> options) {
            Options = options;
        }

        public TileType GetNextTile()
        {
            tilesCleared += 1;
            if (tilesCleared > 40 && tilesCleared % 8 == 0)
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
}
