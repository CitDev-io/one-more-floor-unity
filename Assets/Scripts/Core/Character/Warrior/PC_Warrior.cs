using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
public class PC_Warrior : PlayerCharacter
{
    private readonly string _name;

    public PC_Warrior() {
        _name = "Warrior";
        TileOptions = new List<TileType> {
            TileType.Shield,
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
