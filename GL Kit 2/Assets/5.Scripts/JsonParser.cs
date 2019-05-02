using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public static class JsonParser 
{

    public static DialogueFile ParseJSONFile(string pFileName)
    {
        string[] directory = new string[] {""};
        //Load our file
        TextAsset jsonObject = Resources.Load<TextAsset>(GetJSONPath(pFileName, out directory));

        DialogueFile dialogueFile = new DialogueFile();
        dialogueFile.Name = pFileName;

        //Load a JSONNode from the file.
        JSONNode jsonObj = JSON.Parse(jsonObject.text);

        //Create an array from only the designer-spesified arrays within that JSON file.
        JSONArray arrayNames = jsonObj[Settings.JSON_DEF_DEFINED_ARRAYS].AsArray;
        int numberOfContainers = arrayNames.Count;
        dialogueFile.CreateContainers(numberOfContainers);
        
        //Go through each array in the file
        for (int i = 0; i < numberOfContainers; i++)
        {
            int numberOfFields = jsonObj[arrayNames[i].Value].Count;
            
            //Fetch keys
            JSONNode containerNode = jsonObj[arrayNames[i].Value].AsObject;
            string[] keys = new string[numberOfFields];
            int iterator = 0;
            foreach (KeyValuePair<string, JSONNode> kvp in containerNode)
            {
                keys[iterator] = kvp.Key;
                iterator++;
            }
            
            for (int j = 0; j < numberOfFields; j++)
            {
                
                
                dialogueFile.SetContainerInfo(i, arrayNames[i], keys[j], jsonObj[arrayNames[i].Value][j]);
                Debug.Log("i: " + i + " arrayName[i]: " + arrayNames[i] + " key: " + keys[j] + " val: " +
                          jsonObj[arrayNames[i].Value][j]);
            }
        }      

        return dialogueFile;
    }

    /// <summary>
    /// Takes in a simple level file name and returns the correct path to the JSON file.
    /// </summary>
    /// <param name="pLevel"></param>
    /// <returns></returns>
    public static string GetJSONPath(string pLevel, out string[] oFileNames)
    {
        string language = GameData.GetLanguage().ToString();
        string directory = Settings.PATH_TEXT_FILES + Settings.PATH_DIALOGUE + language.Substring(0, 1).ToUpper() +
                     language.Substring(1).ToLower();
        string path = directory + "/" + pLevel;
        directory = Settings.PATH_ASSETS_RESOURCES + directory;

        Debug.Log("path:" + path + " | dir: " + directory);
        oFileNames = GetFileList(directory);
        return path;
    }

    /// <summary>
    /// Returns a list of the names of each file in our dialogue folder
    /// </summary>
    /// <param name="pDirectory"></param>
    /// <returns></returns>
    private static string[] GetFileList(string pDirectory)
    {
        string[] rawFileNames = Directory.GetFiles(pDirectory);    
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
                Debug.Log("AN: " + areaNames[i]);
            }
            
        }

        return areaNames;
    }

}
