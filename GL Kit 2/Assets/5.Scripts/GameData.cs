using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static string INFO_VERSION_STATE = "Pre-Alpha";
    public static int INFO_VERSION_BUILD_NUMBER = 0;
    public static bool Initialised { get; set; } = false;

    public enum Language
    {
        DUTCH,
        ENGLISH,
        TOTAL
    };

    private static Language gameLanguage = SetLanguage(Language.ENGLISH);

    //Returns itself so an initial set is done at compile time
    public static Language SetLanguage(Language language)
    {
        gameLanguage = language;
        Dialogue.LoadAllText(Settings.LEVEL_NAMES);
        return gameLanguage;
    }

    public static Language GetLanguage()
    {
        return gameLanguage;
    } 
}
