using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
public class RogueFactory : PCFactory
{
    private int _experience;

    public RogueFactory(int experience) {
        _experience = experience;
    }

    public override PlayerCharacter GetPC() {
        return new PC_Rogue(0);
    }
}
}
