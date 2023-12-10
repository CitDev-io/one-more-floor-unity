using System;

public class RandomRoller {
    Random r;
    public RandomRoller() {
        r = new Random();
    }

    public int Roll() {
        return r.Next(1, 100);
    }
}
