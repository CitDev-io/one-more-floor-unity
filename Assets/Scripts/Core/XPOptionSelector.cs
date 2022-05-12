using System;
using System.Collections.Generic;

public class XPOptionSelector 
{
    public PlayerSkillup[] GetXpOptionsForPlayer(PlayerAvatar player) {
        return new PlayerSkillup[]{
            new PlayerSkillup(){
                Name = "ABILITY 1",
                Description = "WILL DO SOMETHING CRAZY!"
            },
            new PlayerSkillup(){
                Name = "ABILITY 2",
                Description = "WILL BE MORE REASONABLE."
            },
            new PlayerSkillup(){
                Name = "LIFE!",
                Description = $"INCREASE MAX HEALTH FROM {player.CalcMaxHp()} TO {player.CalcMaxHp() + 5}",
                HitPoints = 5
            },
            new PlayerSkillup(){
                Name = "HAVE AT THEE!",
                Description = $"INCREASE BASE DAMAGE FROM {player.CalcBaseDamage()} TO {player.CalcBaseDamage() + 1 }",
                BaseDamage = 1
            }
        };
    }
    
}
