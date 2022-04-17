using NUnit.Framework;

public class PlayerAvatarTests
{
    [Test]
    public void KillXPCalculated() {
        var ss = new PlayerAvatar();

        CollectionResult res = ss.CollectKilledMonsters(7);

        Assert.AreEqual(res.Collected, 7);
        Assert.AreEqual(res.Earned, 7);
        Assert.AreEqual(res.BonusRollCount, 4);
        Assert.AreEqual(res.BonusGained, res.SuccessfulBonusRollCount);
    }

    [Test]
    public void BonusXpChanceCalculated() {
        var ss = new PlayerAvatar();
        Assert.AreEqual(ss.BonusXpChance(), 20);
        // when better able to set abritrary, +STR affect this?
    }

    [Test]
    public void BonusCoinChanceCalculated() {
        var ss = new PlayerAvatar();

        Assert.AreEqual(ss.BonusCoinChance(), 20);
        // when better able to set abritrary, +LUC affect this?
    }

    [Test]
    public void BonusShieldChanceCalculated() {
        var ss = new PlayerAvatar();

        Assert.AreEqual(ss.BonusShieldChance(), 20);
        // when better able to set abritrary, +DEX affect this?
    }

    [Test]
    public void BonusPotionChanceCalculated() {
        var ss = new PlayerAvatar();

        Assert.AreEqual(ss.BonusHpChance(), 20);
        // when better able to set abritrary, +VIT affect this?
    }

    // TODO: REQUIRES MOCKING TO PROVE THAT ROLLS EVALUATED CORRECTLY
    // CollectShields x4 need set of tests each
}
