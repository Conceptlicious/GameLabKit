using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public static class JsonParser 
{

    public static DialogueContents ParseJSONFile(string pFileName)
    {
        string[] directory = new string[] {""};
        //Load our file
        TextAsset jsonObject = Resources.Load<TextAsset>(GetJSONPath(pFileName, out directory));
        
        //Create a new contents instance
        DialogueContents contents = new DialogueContents();

        //Load a JSONNode from the file.
        JSONNode jsonObj = JSON.Parse(jsonObject.text);

        //Create an array from only the designer-spesified arrays within that JSON file.
        JSONArray arrayNames = jsonObj[Settings.JSON_DEF_DEFINED_ARRAYS].AsArray;
        
        //Find, store and push the identifier keywords to the content class
        JSONArray identifiers = jsonObj[Settings.JSON_DEF_IDENTIFIER_KEYWORDS].AsArray;

        string[] identiferStrings = new string[identifiers.Count];
        for (int i = 0; i < identifiers.Count; i++)
        {
            identiferStrings[i] = identifiers[i].ToString();
        }
        contents.SetIdentifierKeywords(identiferStrings);

        
        //Go through each array in the file
        for (int i = 0; i < arrayNames.Count; i++)
        {
            //A JSONArray that represents a single array in our file
            JSONArray singleArray = jsonObj[arrayNames[i].Value].AsArray;
            
            //The number of elements present in that array
            int numberOfElements = jsonObj[arrayNames[i].Value].Count;
            Debug.Log("number ele: " + numberOfElements);
            
            //The number of fields present in each element of that array
            int[] numberOfFields = new int[numberOfElements];
            for (int j = 0; j < numberOfElements; j++)
            {
                numberOfFields[j] = jsonObj[arrayNames[i].Value][j].Count;
                Debug.Log("num fields: " + numberOfFields[j]);
            }
            contents.AddDialogueArray(arrayNames[i].ToString(), singleArray, numberOfElements, numberOfFields);
        }

        return contents;
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
