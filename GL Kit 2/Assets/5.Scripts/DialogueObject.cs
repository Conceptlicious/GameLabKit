using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object to be edited through the DialogueEditor script.
public class DialogueObject : MonoBehaviour
{
    //private string dialogueText = Settings.STR_DEFAULT_DIALOGUE;

    public struct TextInfo
    {
        public string dialogueText;
        public int fileIndex;
        public int containerIndex;
        public string fieldName;
        public int fieldIndex;
        
        public TextInfo(int pFileIndex, int pContainerIndex, string pFieldName, int pFieldIndex)
        {       
            fileIndex = pFileIndex;
            containerIndex = pContainerIndex;
            fieldName = pFieldName;
            fieldIndex = pFieldIndex;
            dialogueText = Dialogue.GetText(fileIndex, containerIndex, fieldName);
        }
    }

    private TextInfo info;

    public TextInfo Info
    {
        get { return info; }
        set { info = value; }
    }

    public string GetNextInContainer()
    {
        int newIndex = 0;
        string str = Dialogue.GetNextText(info.fileIndex, info.containerIndex, info.fieldIndex, out newIndex);
        info.dialogueText = str;
        info.fieldIndex = newIndex;
        return str;
    }
    

    /*public string DialogueText
    {
        get { return dialogueText; }
        set { dialogueText = value; }
    }*/


}
