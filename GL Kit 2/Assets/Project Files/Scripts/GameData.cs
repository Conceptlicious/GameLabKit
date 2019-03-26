using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static string INFO_VERSION_STATE = "Pre-Alpha";
    public static int INFO_VERSION_BUILD_NUMBER = 0;

    public enum Language
    {
        DUTCH,
        ENGLISH,
        TOTAL
    };

    private static Language gameLanguage = Language.ENGLISH;

    public static void SetLanguage(Language pLanguage)
    {
        gameLanguage = pLanguage;
    }

    public static Language GetLanguage()
    {
        return gameLanguage;
    }
    
}
