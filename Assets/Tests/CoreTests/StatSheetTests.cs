using NUnit.Framework;

public class StatSheetTests
{
    [Test]
    public void Play() {
        StatSheet a = new StatSheet(
            StatMatrix.BASE_MONSTER(),
            new StatMatrix(){
                Vitality = 8
            }
        );

        Assert.AreEqual(8, a.CalcMaxHp());
        Assert.AreEqual(8, a.Hp);

        PlayerAvatar pa = new PlayerAvatar(
            StatMatrix.BASE_PLAYER(),
            new StatMatrix(){
                Vitality = 8
            }
        );
        Assert.AreEqual(80, pa.CalcMaxHp());
        Assert.AreEqual(80, pa.Hp);
    }
}
