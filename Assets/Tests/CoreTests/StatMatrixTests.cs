using NUnit.Framework;

public class StatMatrixTests
{
    [Test]
    public void ClonableMonster() {
        var monster1 = StatMatrix.BASE_MONSTER();
        var monster2 = StatMatrix.BASE_MONSTER();
        Assert.AreNotSame(monster1, monster2);
        Assert.AreEqual(50, monster1.ArmorDurability);
        Assert.AreEqual(0, monster1.Luck);
    }

        [Test]
    public void ClonablePlayer() {
        var player1 = StatMatrix.BASE_PLAYER();
        var player2 = StatMatrix.BASE_PLAYER();
        Assert.AreNotSame(player1, player2);
        Assert.AreEqual(50, player1.ArmorDurability);
        Assert.AreEqual(4, player1.Defense);
    }
}
