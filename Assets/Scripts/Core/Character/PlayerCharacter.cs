using System.Collections.Generic;
using System;

public abstract class PlayerCharacter : IExperiencable
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

   public void GainExp(IExperiencable expSheet) {
      ExpPoints += expSheet.ExpPoints;
      SwordExpPoints += expSheet.SwordExpPoints;
      HeartExpPoints += expSheet.HeartExpPoints;
      SpecialExpPoints += expSheet.SpecialExpPoints;
   }

   public List<TileType> TileOptions { get; set; }
}
