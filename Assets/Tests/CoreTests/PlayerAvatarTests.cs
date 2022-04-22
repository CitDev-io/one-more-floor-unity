using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;


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
        var ss = new PlayerAvatar(
            StatMatrix.BASE_PLAYER()
        );
        Assert.AreEqual(20, ss.BonusXpChance());
        // when better able to set abritrary, +STR affect this?
    }

    [Test]
    public void BonusCoinChanceCalculated() {
        var ss = new PlayerAvatar(
            StatMatrix.BASE_PLAYER()
        );

        Assert.AreEqual(20, ss.BonusCoinChance());
        // when better able to set abritrary, +LUC affect this?
    }

    [Test]
    public void BonusShieldChanceCalculated() {
        var ss = new PlayerAvatar(
            StatMatrix.BASE_PLAYER()
        );

        Assert.AreEqual(20, ss.BonusShieldChance());
        // when better able to set abritrary, +DEX affect this?
    }

    [Test]
    public void BonusPotionChanceCalculated() {
        var ss = new PlayerAvatar(
            StatMatrix.BASE_PLAYER()
        );
        Assert.AreEqual(20, ss.BonusHpChance());
        // when better able to set abritrary, +VIT affect this?
    }

    // TODO: REQUIRES MOCKING TO PROVE THAT ROLLS EVALUATED CORRECTLY
    // CollectShields x4 need set of tests each


    [Test]
    public void MessAround() {
        Dictionary<ItemSlot, PlayerItem> Inventory = new Dictionary<ItemSlot, PlayerItem>();
        StatMatrix[] allStats = Inventory.Select(z => (StatMatrix) z.Value).ToArray();

        Assert.AreEqual(0, allStats.Count());
        allStats = allStats.Append(StatMatrix.BASE_PLAYER()).ToArray();
        Assert.AreEqual(1, allStats.Count());

        var reduced = StatMatrix.Reduce(allStats);
        Assert.AreEqual(1, reduced.Strength);
    }
}
