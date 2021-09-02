using NUnit.Framework;

public class PC_WarriorTest
{
    [Test]
    public void HasCorrectTileCount()
    {
        PC_Warrior w = new PC_Warrior();

        Assert.AreEqual(4, w.TileOptions.Count);
    }
}
