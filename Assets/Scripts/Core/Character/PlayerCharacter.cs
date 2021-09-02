using System.Collections.Generic;

public abstract class PlayerCharacter
{
   public abstract string Name { get; }

   public int ExpPoints { get; set; }
   public int SwordExpPoints { get; set; }
   public int HeartExpPoints { get; set; }
   public int GoldExpPoints { get; set; }
   public int SpecialExpPoints { get; set; }

   public int Damage { get; set; } = 0;
   public int MaxHitPoints { get; set; } = 10;
   public int MaxSpecialPoints { get; set; } = 10;

   public int HitPoints { get; set; } = 10;
   public int SpecialPoints { get; set; } = 0;

   public List<TileType> TileOptions { get; set; }
}
