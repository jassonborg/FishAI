using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic
{
    //Calculate experience required to level up
    public static float ExperienceForNextLevel(int currentLevel)
    {
        if(currentLevel == 0) return 0;
        return (currentLevel * currentLevel + currentLevel + 10) * 5;
    }
}
