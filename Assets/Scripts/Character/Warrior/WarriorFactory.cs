using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
public class WarriorFactory : PCFactory
{
    private int _experience;

    public WarriorFactory(int experience) {
        _experience = experience;
    }

    public override PlayerCharacter GetPC() {
        return new PC_Warrior(0);
    }
}
}
