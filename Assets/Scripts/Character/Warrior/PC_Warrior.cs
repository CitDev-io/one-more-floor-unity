using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
public class PC_Warrior : PlayerCharacter
{
    private readonly string _name;
    private int _experience;

    public PC_Warrior(int exp) {
        _name = "Warrior";
        _experience = exp;
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

    public override int Experience {
        get { return _experience; }
        set { _experience = value; }
    }
}
}
