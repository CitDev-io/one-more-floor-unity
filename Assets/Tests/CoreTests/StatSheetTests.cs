using NUnit.Framework;

public class StatSheetTests
{
    [Test]
    public void StartingStatsUsed()
    {
        var ss = new StatSheet();
        
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

    [Test]
    public void KillXPCalculated() {
        var ss = new StatSheet();

        CollectionResult res = ss.CollectKilledMonsters(7);

        Assert.AreEqual(res.Collected, 7);
        Assert.AreEqual(res.Earned, 7);
        Assert.AreEqual(res.BonusRollCount, 4);
        Assert.AreEqual(res.BonusGained, res.SuccessfulBonusRollCount);
    }

    [Test]
    public void BonusXpChanceCalculated() {
        var ss = new StatSheet();
        Assert.AreEqual(ss.BonusXpChance(), 20);
        // when better able to set abritrary, +STR affect this?
    }

    [Test]
    public void BonusCoinChanceCalculated() {
        var ss = new StatSheet();

        Assert.AreEqual(ss.BonusCoinChance(), 20);
        // when better able to set abritrary, +LUC affect this?
    }

    [Test]
    public void BonusShieldChanceCalculated() {
        var ss = new StatSheet();

        Assert.AreEqual(ss.BonusShieldChance(), 20);
        // when better able to set abritrary, +DEX affect this?
    }

    [Test]
    public void BonusPotionChanceCalculated() {
        var ss = new StatSheet();

        Assert.AreEqual(ss.BonusHpChance(), 20);
        // when better able to set abritrary, +VIT affect this?
    }

    // TODO: REQUIRES MOCKING TO PROVE THAT ROLLS EVALUATED CORRECTLY
    // CollectShields x4 need set of tests each
}
