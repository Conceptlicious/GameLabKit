﻿
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEditor;
using System.IO;
using System.Linq;

/// <summary>
///  
/// </summary>
public static class Dialogue 
{
    private static DialogueFile[] files = null;

    public static void LoadAllText(string[] pLevelNames)
    {
        int size = pLevelNames.Length;
        files = new DialogueFile[size];
        for (int i = 0; i < size; i++)
        {
            files[i] = JsonParser.ParseJSONFile(pLevelNames[i]);
        }
        
        //Turn into event
        GameData.Initialised = true;
    }

    public static string[] GetFileNames()
    {
        string[] names = new string[1];
        if (files != null)
        {
            names = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                names[i] = files[i].Name;
            }
        }
        else
        {
            names[0] = Settings.STR_DEFAULT_DIALOGUE;
        }

        return names;
    }

    /*private static string[] TrawlArrayForNameProperty<T>(T[] pArray)
    {
        string[] names = new string[pArray.Length];
        for (int i = 0; i < pArray.Length; i++)
        {
            names[i] = pArray[i].Name;
        }

        return names;
    }*/

    public static string[] GetContainerNames(int pFileID)
    {  
        DialogueContainer[] containers = files[pFileID].GetContainers();
        string[] names = new string[containers.Length];
        for (int i = 0; i < containers.Length; i++)
        {
            names[i] = containers[i].Name;
        }

        return names;
    }

    public static string[] GetKeyNames(int pFileID, int pContainerID)
    {
        DialogueContainer container = files[pFileID].GetContainer(pContainerID);
        Dictionary<string, string> kvp = container.GetInfoDictionary();
        string[] names = new string[kvp.Count];
        int iterator = 0;
        foreach (KeyValuePair<string, string> pair in kvp)
        {
            names[iterator] = pair.Key;
            iterator++;
        }

        return names;
    }

    public static string GetText(int pFileID, int pContainerID, string pField)
    {
        string text = Settings.STR_DEFAULT_DIALOGUE;
        files[pFileID].GetContainer(pContainerID).GetInfoDictionary().TryGetValue(pField, out text);
        return text;
    }

    public static string GetNextText(int pFileID, int pContainerID, int pFieldIndex, out int pNewIndex)
    {
        string[] keyNames = GetKeyNames(pFileID, pContainerID);
        
        pNewIndex = (pFieldIndex + 1) % keyNames.Length;
        return GetText(pFileID, pContainerID, keyNames[pNewIndex]);
    }

    public static string GetTextAt(int pFileID, int pContainerID, int pFieldIndex)
    {

        string[] keyNames = GetKeyNames(pFileID, pContainerID);
        pFieldIndex = pFieldIndex >= 0 && pFieldIndex < keyNames.Length ? pFieldIndex : -1;
        string text = pFieldIndex == -1
            ? Settings.ERR_DIALOGUE_INVALID_INDEX
            : GetText(pFileID, pContainerID, keyNames[pFieldIndex]);
        return text;
    }

   
    

   
    //when language change event fires, reload dialogue
    public static void onLanguageChange()
    {
        //Debug.Assert(pID >= 0 && pID < (int)GameData.Language.TOTAL);
        //Debug.Log("----------------------------------------");
        //TO DO: Should be on language change event
        //GameData.SetLanguage((GameData.Language)pID);
        //LoadAreaDialogue(currentAreaName);
        
        /*
         *
         * Room dialogue is just clicking next, and fetching 

Can break with newlines

Each in-scene object has script, use dropdowns to select "dialogue"

White room JSON file has array with name of each object, and dialogue GameLab would say if you chose that

         */
    }
    
}
