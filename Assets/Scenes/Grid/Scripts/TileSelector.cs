using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
    public class TileSelector
    {
        int tilesCleared = 0;

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
            int tileChoice = Random.Range(0, 4);
            return (TileType)tileChoice;
        }
    }
}
