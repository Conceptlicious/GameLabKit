using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
/// <summary>
/// The full text content from a level file.
/// </summary>
[Serializable]
public class DialogueContents
{
    //Dict of <name of the array, list of its elements>
    //Each item in the list is a Dictionary holding all the keys and values the fields present in that element
    //Dictionary<string, List<Dictionary<string, string>>>
    public Dictionary<string, List<Dictionary<string, string>>> allArrays;
    private string[] identifierKeywords;

    public DialogueContents()
    {
        allArrays = new Dictionary<string, List<Dictionary<string, string>>>();
    }

    public string[] GetNames()
    {
        string[] names = new string[allArrays.Count];
        int iterator = 0;
        foreach (KeyValuePair<string, List<Dictionary<string, string>>> kvp in allArrays)
        {
            names[iterator] = kvp.Key;
            iterator++;
        }

        return names;
    }

    //NOTE: I should probably make a generic way for trawling the tree and pulling data out
    public string GetLine(string pArrayName, int pElement, string pField)
    {
        string output = "";
        if (allArrays.ContainsKey(pArrayName))
        {
            List<Dictionary<string, string>> elements = allArrays[pArrayName];
            if (pElement >= 0 && pElement < elements.Count)
            {                
                if (allArrays[pArrayName][pElement].TryGetValue(pField, out output))
                {
                   
                }
                else
                {
                    //key not found exception
                }
               
            }       
        }

        return output;
    }
    
    public string[] GetKeywords(string pArrayName)
    {
        //"Blank" for now
        string[] keywords = new string[] { Settings.STR_DEFAULT_DIALOGUE };
        //Does an array of that name exist?
        if (allArrays.ContainsKey(pArrayName))
        {
            //Pre-Calc
            int length = allArrays[pArrayName].Count;
            
            keywords = new string[length];
            
            //Go through elements
            for (int i = 0; i < length; i++)
            {
                string keywordValue = "";
                List<Dictionary<string, string>> elements;
                
                    //If an array of that name exits
                    if (allArrays.TryGetValue(pArrayName, out elements))
                    {
                        Debug.Log("array '" + pArrayName + "' found.");
                        //Look only for fields that match the identifier (e.g.: "name" or "tag")
                        for (int j = 0; j < identifierKeywords.Length; j++)
                        {
                            if (elements[i].TryGetValue(identifierKeywords[j].Trim('"'), out keywordValue))
                            {
                                Debug.Log("element " + i + " with identifer " + identifierKeywords[j] + " has val: " + keywordValue);
                                keywords[i] = keywordValue;
                                break;
                            }
                            else
                            {
                                Debug.Log("nothing found in " + pArrayName + ", element " + i + " with identifier: " + identifierKeywords[j]);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("no array by name " + pArrayName + " found.");
                    }
          
                
            }
        }

        return keywords;
    }

    public void SetIdentifierKeywords(string[] pIdentifierKeywords)
    {
        identifierKeywords = pIdentifierKeywords;
    }
    

    /// <summary>
    /// Adds a List representing a full array of data to a Dictionary of every array present in the given file (and their names).
    /// </summary>
    /// <param name="pJsonArrayName"></param>
    /// <param name="pJsonArray"></param>
    /// <param name="pNumberOfElements"></param>
    /// <param name="pNumberOfFields"></param>
    public void AddDialogueArray(string pJsonArrayName, JSONArray pJsonArray, int pNumberOfElements, int[] pNumberOfFields)
    {

        //Create a List representing each element that makes up the array.
        List<Dictionary<string, string>> arrayElements = new List<Dictionary<string, string>>();
        for (int i = 0; i < pNumberOfElements; i++)
        {  
            //Create a Dictionary storing a key-value pair, representing the name and data from each field in an array element.
            Dictionary<string, string> arrayData = new Dictionary<string, string>();
            
            //Pull the key names from the JSONNode since they are not stored in the JSONArray
            JSONNode node = pJsonArray[i].AsObject;
            string[] keys = new string[pNumberOfFields[i]];
            int iterator = 0;
            foreach (KeyValuePair<string, JSONNode> kvp in node)
            {
                keys[iterator] = kvp.Key;
                iterator++;
            }
            
            //Go through each field in a single element and add the keys and values to a dictionary.
            for (int j = 0; j < pNumberOfFields[i]; j++)
            {

                string val = pJsonArray[i][j].Value;
                Debug.Log("Key: " + keys[j] + " | Val: " + val);
                arrayData.Add(keys[j], val);
            }
            //Add all the elements
            arrayElements.Add(arrayData);
        }
        //Add the List which now represents one present array to a Dictionary of each array defined in the file.
        allArrays.Add(pJsonArrayName, arrayElements);
    }
}
