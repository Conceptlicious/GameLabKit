using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __RemoveScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //This should be done on a game start event, not hardcoded here. Currently just for testing purposes.
        GameData.SetLanguage(GameData.Language.ENGLISH);
        string[] levelNames = { "Room_1", "Room_2" };
        Dialogue.LoadAllText(levelNames);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
