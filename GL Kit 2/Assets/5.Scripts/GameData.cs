using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This static script is meant to hold all values that refer to the game itself, including the versioning
//and current language options.
//Usage: Used globally.
//--------------------------------------------------


public static class GameData
{
    public static string INFO_VERSION_STATE = "Pre-Alpha";
    public static int INFO_VERSION_BUILD_NUMBER = 0;
    private static bool initialised = false;
    
    public static bool Initialised 
    { 
        get => initialised;
        set { initialised = value; }
    }

    public enum Language
    {
        DUTCH,
        ENGLISH,
        TOTAL
    };

    private static Language gameLanguage = Language.DUTCH;

    //Returns itself so an initial set is done at compile time
    public static void SetLanguage(Language language)
    {
        gameLanguage = language;
        initialised = true;
    }

    public static Language GetLanguage()
    {
        return gameLanguage;
    } 
}
