using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
public abstract class PlayerCharacter
{
   public abstract string Name { get; }
   public abstract int Experience { get; set; }
   public List<TileType> TileOptions { get; set; }
}
}
