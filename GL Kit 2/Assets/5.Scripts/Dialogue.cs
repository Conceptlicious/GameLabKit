
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEditor;
using System.IO;
using System.Linq;
using Random = System.Random;

/// <summary>
///  
/// </summary>
public static class Dialogue 
{
    private static DialogueFile[] files = null;

    /// <summary>
    /// Load all the available dialogue files for the current language.
    /// </summary>
    /// <param name="pLevelNames"></param>
    public static void LoadAllText()
    {
        int size = Settings.LEVEL_NAMES.Length;
        files = new DialogueFile[size];
        for (int i = 0; i < size; i++)
        {
            files[i] = JsonParser.ParseJSONFile(Settings.LEVEL_NAMES[i]);
        }

        Debug.Log("Reloading all files.");

        //Turn into event
        GameData.Initialised = true;
    }

    /// <summary>
    /// Get a string array of the files currently loaded;
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Get the string names of the arary containers used to wrap different chunks of text inside a single level file.
    /// </summary>
    /// <param name="pFileID"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Get the names of the keys used to preceed the individual lines of text.
    /// </summary>
    /// <param name="pFileID"></param>
    /// <param name="pContainerID"></param>
    /// <returns></returns>
    public static string[] GetKeyNames(int pFileID, int pContainerID)
    {
        if (files == null)
        {
            Debug.Log("Files null");
        }


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

    /// <summary>
    /// Get a particular line of text by directly calling the file name and container name.
    /// </summary>
    /// <param name="pFileID"></param>
    /// <param name="pContainerID"></param>
    /// <param name="pField"></param>
    /// <returns></returns>
    public static string GetText(int pFileID, int pContainerID, string pField)
    {
        string text = Settings.STR_DEFAULT_DIALOGUE;
        files[pFileID].GetContainer(pContainerID).GetInfoDictionary().TryGetValue(pField, out text);
        return text;
    }

    /// <summary>
    /// Get the next line of text from a container. Iterates and wraps the passed index.
    /// </summary>
    /// <param name="pFileID"></param>
    /// <param name="pContainerID"></param>
    /// <param name="pFieldIndex"></param>
    /// <param name="pNewIndex"></param>
    /// <returns></returns>
    public static string GetTextAndIterate(int pFileID, int pContainerID, int pFieldIndex, out int pNewIndex)
    {
        string[] keyNames = GetKeyNames(pFileID, pContainerID);
        int oldIndex = pFieldIndex;
        pNewIndex = (pFieldIndex + 1) % keyNames.Length;
       // Debug.Log("New index is " + pNewIndex);
        return GetText(pFileID, pContainerID, keyNames[oldIndex]);
    }

    /// <summary>
    /// Get a line of text from a container using a specific index.
    /// </summary>
    /// <param name="pFileID"></param>
    /// <param name="pContainerID"></param>
    /// <param name="pFieldIndex"></param>
    /// <returns></returns>
    public static string GetTextAt(int pFileID, int pContainerID, int pFieldIndex)
    {

        string[] keyNames = GetKeyNames(pFileID, pContainerID);
        pFieldIndex = pFieldIndex >= 0 && pFieldIndex < keyNames.Length ? pFieldIndex : -1;
        string text = pFieldIndex == -1
            ? Settings.ERR_DIALOGUE_INVALID_INDEX
            : GetText(pFileID, pContainerID, keyNames[pFieldIndex]);
        return text;
    }

    /// <summary>
    /// Get a random line of text from the given container.
    /// </summary>
    /// <param name="pFileID"></param>
    /// <param name="pContainerID"></param>
    /// <returns></returns>
    public static string GetRandomText(int pFileID, int pContainerID)
    {
        string[] keyNames = GetKeyNames(pFileID, pContainerID);       
        string text = GetTextAt(pFileID, pContainerID, UnityEngine.Random.Range(0, keyNames.Length));
        return text;
    }



    public static int GetMaxFieldIndices(int pFileID, int pContainerID)
    {
        string[] keyNames = GetKeyNames(pFileID, pContainerID);       
        return keyNames.Length;
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
