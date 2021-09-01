using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace citdev
{
    public class PC_WarriorTest
    {
        [Test]
        public void HasCorrectTileCount()
        {
            PC_Warrior w = new PC_Warrior();

            Assert.AreEqual(4, w.TileOptions.Count);
        }
    }
}
