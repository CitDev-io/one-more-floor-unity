using NUnit.Framework;

public class StatSheetTests
{
    [Test]
    public void StartingStatsUsed()
    {
        var ss = new StatSheet() {
            Strength = 3,
            
        };
        
        Assert.AreEqual(ss.Strength, 1);
        Assert.AreEqual(ss.Dexterity, 1);
        Assert.AreEqual(ss.Vitality, 1);
        Assert.AreEqual(ss.Luck, 1);
        Assert.AreEqual(ss.WeaponDamage, 1); // will be 0 when starter gear introduced
        Assert.AreEqual(ss.ArmorPiercing, 50);
        Assert.AreEqual(ss.ArmorDurability, 50);
        Assert.AreEqual(ss.Defense, 4); // will be 0 when starter gear introduced
    }

    [Test]
    public void BaseDamageIsCalculated() {
        var ss = new StatSheet();

        Assert.AreEqual(ss.Strength + 2, ss.CalcBaseDamage());
    }

    [Test]
    public void DamageDoneIsCalculated() {
        var ss = new StatSheet();

        Assert.AreEqual(ss.CalcDamageDone(6), 9);
    }
}
