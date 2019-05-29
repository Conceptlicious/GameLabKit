using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class LevelProgressEvent : GameLabEvent
{
    public int levelID;
    public LevelProgressEvent(int pLevelID)
    {
        levelID = pLevelID;
    }
}
