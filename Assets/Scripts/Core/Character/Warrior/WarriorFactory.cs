using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
public class WarriorFactory : PCFactory
{
    public WarriorFactory() {
    }

    public override PlayerCharacter GetPC() {
        return new PC_Warrior();
    }
}
}
