using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Key data, allowing
/// </summary>
public class DialogueObject : MonoBehaviour
{
    private string arrayName = "";
    private int elementIndex = 0;
    private string fieldName = "";
    
    // Start is called before the first frame update
    void Start()
    {
        GameData.SetLanguage(GameData.Language.ENGLISH);
        Dialogue.LoadAreaDialogue("Room_1");
    }

    public void SetKeys(string pArrayName, int pElementIndex, string pFieldName)
    {
        arrayName = pArrayName;
        elementIndex = pElementIndex;
        fieldName = pFieldName;
    }
}
