using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueContainer
{
    private string name = Settings.STR_DEFAULT_DIALOGUE;
    private Dictionary<string, string> info = new Dictionary<string, string>();
    

    public void SetInfo(string pKey, string pValue)
    {
        info.Add(pKey, pValue);
    }

    public Dictionary<string, string> GetInfoDictionary()
    {
        return info;
    }
    

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
}
