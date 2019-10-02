using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This parses a given json file and stores it's info in our DialogueFile objects.
//Usage: Used as part of the dialogue system.
//--------------------------------------------------

public static class JsonParser 
{

    public static DialogueFile ParseJSONFile(string fileName)
    {
        string[] directory = new string[] {string.Empty};
        
        // Load our file
        TextAsset jsonObject = Resources.Load<TextAsset>(GetJSONPath(fileName, out directory));
        Debug.Assert(jsonObject != null, System.String.Format(Settings.ERR_JSON_MISSING_FILE, fileName));
        
        //unnesscary extra safety return
        if (jsonObject == null)
        {
            return null;
        }
        
        //Create the object to be returned.
        DialogueFile dialogueFile = new DialogueFile();
        dialogueFile.Name = fileName;

        // Load a JSONNode from the file.
        JSONNode jsonObj = JSON.Parse(jsonObject.text);

        // Create an array from only the designer-specified arrays within that JSON file.
        JSONArray arrayNames = jsonObj[Settings.JSON_DEF_DEFINED_ARRAYS].AsArray;
        int numberOfContainers = arrayNames.Count;
        
        // Create the DialogueContainers the DialogueFile will own.
        dialogueFile.CreateContainers(numberOfContainers);
        
        // Go through each array in the file
        for (int i = 0; i < numberOfContainers; i++)
        {
            int numberOfFields = jsonObj[arrayNames[i].Value].Count;
            
            //Fetch keys
            JSONNode containerNode = jsonObj[arrayNames[i].Value].AsObject;
            
            //Get key names since they are not accessible else wise.
            string[] keys = new string[numberOfFields];
            Debug.Assert(keys != null && numberOfFields != 0, System.String.Format(Settings.ERR_JSON_FILE_INCORRECT_FORMATTING, fileName));
            int iterator = 0;
            foreach (KeyValuePair<string, JSONNode> kvp in containerNode)
            {
                keys[iterator] = kvp.Key;
                iterator++;
            }
            
            //Save our info
            for (int j = 0; j < numberOfFields; j++)
            {
                dialogueFile.SetContainerInfo(i, arrayNames[i], keys[j], jsonObj[arrayNames[i].Value][j]);
                //Debug.Log("i: " + i + " arrayName[i]: " + arrayNames[i] + " key: " + keys[j] + " val: " +
                          //jsonObj[arrayNames[i].Value][j]);
            }
        }      

        return dialogueFile;
    }

    /// <summary>
    /// Takes in a simple level file name and returns the correct path to the JSON file.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static string GetJSONPath(string level, out string[] fileNames)
    {
        string language = GameData.GetLanguage().ToString();
        string directory = Settings.PATH_TEXT_FILES + Settings.PATH_DIALOGUE + language.Substring(0, 1).ToUpper() +
                     language.Substring(1).ToLower();
        string path = directory + "/" + level;
        directory = Settings.PATH_ASSETS_RESOURCES + directory;

        //Debug.Log("path:" + path + " | dir: " + directory);
        fileNames = GetFileList(directory);
        return path;
    }

    /// <summary>
    /// Returns a list of the names of each file in our dialogue folder
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    private static string[] GetFileList(string directory)
    {
        string[] rawFileNames = Directory.GetFiles(directory);    
        int numberOfFiles = rawFileNames.Length;
       
        //Names of our "levels"
        string[] areaNames = new string[numberOfFiles];
        
        for (int i = 0; i < numberOfFiles; i++)
        {
            string[] directory_FileNameSplit = rawFileNames[i].Split('\\');
            string[] fileName_FileTypeSplit = directory_FileNameSplit[1].Split('.');
            if (fileName_FileTypeSplit[1] != Settings.FLTYPE_JSON_META)
            {
                areaNames[i] = fileName_FileTypeSplit[0];
            }
            
        }

        return areaNames;
    }

}
