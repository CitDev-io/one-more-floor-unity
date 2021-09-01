using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
public class RogueFactory : PCFactory
{
    public RogueFactory() {
        
    }

    public override PlayerCharacter GetPC() {
        return new PC_Rogue();
    }
}
}
