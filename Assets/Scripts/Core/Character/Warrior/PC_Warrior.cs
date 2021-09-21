using System.Collections.Generic;
using System;

public class PC_Warrior : PlayerCharacter
{
    private readonly string _name;

    public PC_Warrior() {
        _name = "Warrior";
        BaseHp = 14;
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

    public override StatSheet GetStatSheet()
    {
        return new StatSheet(MaxHp(), MaxSp(), Damage());
    }

    int Damage() {
        return 2 + (int) Math.Floor(SwordExpPoints/ 220d);
    }

    int MaxHp()
    {
        int heartLevels = (int) Math.Floor(HeartExpPoints / 100d);
        return BaseHp + (heartLevels * 3);
    }

    int MaxSp()
    {
        int spLevels = (int) Math.Floor(SpecialExpPoints / 80d);
        return 10 + (spLevels * 3);
    }
}
