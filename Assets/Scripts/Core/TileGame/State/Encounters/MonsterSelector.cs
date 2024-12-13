using System;

public class MonsterSelector 
{
    
    Random r = new Random();

    public StatSheet NextMonster(ITileCollectorContext context) {
        int Strength = 2;
        int Defense = 4;
        int Vitality = 3;

        int tier = 1;
        if (context != null) {
            tier = context.MovesMade / 26;
            UnityEngine.Debug.Log("tier: " + tier);
        } else {
            UnityEngine.Debug.Log("What? No context?");
        }
        switch(tier) {
            case 0:
                Strength = 1;
                Defense = 0;
                Vitality = 2;
            break;
            case 1:
                Strength = 1;
                Defense = 0;
                Vitality = 4;
            break;
            case 2:
                Strength = r.Next(1, 2);
                Defense = r.Next(0, 1);
                Vitality = 4;
            break;
            case 3:
                Strength = r.Next(1, 2);
                Defense = r.Next(1, 2);
                Vitality = r.Next(4, 6);
            break;
            case 4:
                Strength = 2;
                Defense = 2;
                Vitality = r.Next(5, 7);
            break;
            case 5:
                Strength = r.Next(2, 3);
                Defense = r.Next(2, 3);
                Vitality = r.Next(6, 8);
            break;
            case 6:
                Strength = r.Next(2, 3);
                Defense = r.Next(2, 3);
                Vitality = r.Next(6, 9);
            break;
            case 7:
                Strength = 3;
                Defense = r.Next(3, 4);
                Vitality = r.Next(8, 11);
            break;
            case 8:
                Strength = r.Next(2, 4);
                Defense = r.Next(3, 5);
                Vitality = r.Next(9, 13);
            break;
            case 9:
                Strength = 4;
                Defense = r.Next(4, 6);
                Vitality = r.Next(11, 15);
            break;
            case 10:
                Strength = r.Next(3, 4);
                Defense = r.Next(4, 6);
                Vitality = r.Next(12, 17);
            break;
            case 11:
                Strength = r.Next(3, 5);
                Defense = r.Next(4, 7);
                Vitality = r.Next(13, 19);
            break;
            case 12:
                Strength = r.Next(4, 5);
                Defense = r.Next(5, 7);
                Vitality = r.Next(15, 20);
            break;
            default:
                Strength = 5;
                Defense = 7;
                Vitality = 20;
            break;
        }

        return new StatSheet(new StatMatrix(){
                Vitality = Vitality,
                Strength = Strength,
                Defense = Defense
            });
    }
}
