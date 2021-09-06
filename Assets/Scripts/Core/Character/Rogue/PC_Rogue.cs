using System.Collections.Generic;
using System;

public class PC_Rogue : PlayerCharacter
{
    private readonly string _name;

    public PC_Rogue() {
        _name = "Rogue";
        BaseHp = 10;
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

    public override StatSheet GetStatSheet()
    {
        return new StatSheet(MaxHp(), MaxSp(), Damage());
    }

    int Damage() {
        return 1 + (int) Math.Floor(SwordExpPoints/ 150d);
    }

    int MaxHp()
    {
        int heartLevels = (int) Math.Floor(HeartExpPoints / 40d);
        return BaseHp + (heartLevels * 2);
    }

    int MaxSp()
    {
        int spLevels = (int) Math.Floor(SpecialExpPoints / 40d);
        return 6 + (spLevels);
    }
}
