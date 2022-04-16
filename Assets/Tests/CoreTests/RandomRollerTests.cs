using NUnit.Framework;

public class RandomRollerTests
{
    [Test]
    public void GivesGoodValues()
    {
        RandomRoller rr = new RandomRoller();

        int roll1 = rr.Roll();
        Assert.True(roll1 > 0 && roll1 < 101);
    }
}
