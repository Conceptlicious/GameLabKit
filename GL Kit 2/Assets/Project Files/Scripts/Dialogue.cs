
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEditor;
using System.IO;

/// <summary>
///  
/// </summary>
public static class Dialogue 
{
    private static DialogueContents contents = null;
    private static int index = 0;

    //TO DO: Change from hard-coded reference
    private static string currentAreaName = "Room_1";
    // Start is called before the first frame update   

    public static void LoadAreaDialogue(string pAreaName)
    {
        currentAreaName = pAreaName;
        contents = JsonParser.ParseJSONFile(pAreaName);
    }
    
    //when language change event fires, reload dialogue
    public static void onLanguageChange()
    {
        //Debug.Assert(pID >= 0 && pID < (int)GameData.Language.TOTAL);
        //Debug.Log("----------------------------------------");
        //TO DO: Should be on language change event
        //GameData.SetLanguage((GameData.Language)pID);
        LoadAreaDialogue(currentAreaName);
        
        /*
         *
         * Room dialogue is just clicking next, and fetching 

Can break with newlines

Each in-scene object has script, use dropdowns to select "dialogue"

White room JSON file has array with name of each object, and dialogue GameLab would say if you chose that

         */
    }

    public static string[] GetArrayListings()
    {
        string[] output = new string[] { Settings.STR_DEFAULT_DIALOGUE };
        if (contents != null)
        {
            output = contents.GetNames();
        }       
        return output;
    }


    public static string[] GetKeywordListings(string pArrayName)
    {
        string[] output = new string[] { Settings.STR_DEFAULT_DIALOGUE };
        if (contents != null)
        {
            output = contents.GetKeywords(pArrayName);
        }       
        return output;
    }

    public static string[] GetLevelFileListings()
    {
        string[] output = new string[] { Settings.STR_DEFAULT_DIALOGUE };
        JsonParser.GetJSONPath("", out output);
        return output;
    }
}
