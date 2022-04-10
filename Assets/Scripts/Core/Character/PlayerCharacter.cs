using System.Collections.Generic;
using System;

public abstract class PlayerCharacter
{
   public abstract string Name { get; }

   public int ExpPoints { get; set; }
   public int SwordExpPoints { get; set; }
   public int HeartExpPoints { get; set; }
   public int SpecialExpPoints { get; set; }

   public int BaseHp { get; set; }
   public abstract StatSheet GetStatSheet();

   public int Level() {
      return 1 + (int) Math.Floor(ExpPoints / 100d);
   }

   public List<TileType> TileOptions { get; set; }
}
