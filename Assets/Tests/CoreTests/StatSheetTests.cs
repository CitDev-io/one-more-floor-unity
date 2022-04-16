using NUnit.Framework;

public class StatSheetTests
{
    [Test]
    public void StartingStatsUsed()
    {
        var ss = new StatSheet(10, 10, 1, 0);
        
        Assert.AreEqual(ss.Strength, 1);
        Assert.AreEqual(ss.Dexterity, 1);
        Assert.AreEqual(ss.Vitality, 1);
        Assert.AreEqual(ss.Luck, 1);
        Assert.AreEqual(ss.WeaponDamage, 1);
        Assert.AreEqual(ss.ArmorPiercing, 50);
        Assert.AreEqual(ss.ArmorDurability, 50);
        Assert.AreEqual(ss.Defense, 1);
    }
}
