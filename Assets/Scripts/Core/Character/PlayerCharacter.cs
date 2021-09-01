using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
public abstract class PlayerCharacter
{
   public abstract string Name { get; }

   public int ExpPoints { get; set; }
   public int SwordExpPoints { get; set; }
   public int HeartExpPoints { get; set; }
   public int GoldExpPoints { get; set; }
   public int SpecialExpPoints { get; set; }

   public int Damage { get; set; }
   public int MaxHitPoints { get; set; }
   public int MaxSpecialPoints { get; set; }

   public int HitPoints { get; set; }
   public int SpecialPoints { get; set; }

   public List<TileType> TileOptions { get; set; }
}
}
