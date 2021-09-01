using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
public class PC_Rogue : PlayerCharacter
{
    private readonly string _name;

    public PC_Rogue() {
        _name = "Rogue";
        TileOptions = new List<TileType>{
            TileType.Poison,
            TileType.Sword,
            TileType.Heart,
            TileType.Coin
        };
    }

    public override string Name {
        get { return _name; }
    }
}
}
