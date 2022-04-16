using NUnit.Framework;

public class StatSheetTests
{
    [Test]
    public void StartingStatsUsed()
    {
        var ss = new StatSheet(10, 10, 0);
        
        Assert.AreEqual(ss.Strength, 1);
        Assert.AreEqual(ss.Dexterity, 1);
        Assert.AreEqual(ss.Vitality, 1);
        Assert.AreEqual(ss.Luck, 1);
        Assert.AreEqual(ss.WeaponDamage, 1);
        Assert.AreEqual(ss.ArmorPiercing, 50);
        Assert.AreEqual(ss.ArmorDurability, 50);
        Assert.AreEqual(ss.Defense, 1);
    }

    [Test]
    public void BaseDamageIsCalculated() {
        var ss = new StatSheet(10, 10, 0);

        Assert.AreEqual(ss.Strength + 2, ss.CalcBaseDamage());
    }

    [Test]
    public void DamageDoneIsCalculated() {
        var ss = new StatSheet(10, 10, 0);

        Assert.AreEqual(ss.CalcDamageDone(6), 9);
    }

    [Test]
    public void KillXPCalculated() {
        var ss = new StatSheet(10, 10, 0);

        MonsterCollectionResult res = ss.CollectKilledMonsters(7);

        Assert.AreEqual(res.Collected, 7);
        Assert.AreEqual(res.XPEarned, 7);
        Assert.AreEqual(res.BonusXPRollCount, 4);
        Assert.AreEqual(res.BonusXPGained, res.SuccessfulBonusXPRollCount);
    }

    [Test]
    public void BonusXpChanceCalculated() {
        var ss = new StatSheet(10, 10, 0);

        Assert.AreEqual(ss.BonusXpChance(), 20);
    }

    //REQUIRES MOCKING TO PROVE THAT ROLLS EVALUATED CORRECTLY
}
