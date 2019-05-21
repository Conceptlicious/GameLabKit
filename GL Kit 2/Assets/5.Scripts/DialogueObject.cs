using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object to be edited through the DialogueEditor script.
public class DialogueObject : MonoBehaviour
{
    //private string dialogueText = Settings.STR_DEFAULT_DIALOGUE;

    public struct TextInfo
    {
        private string dialogueText;
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

        public string DialogueText
        {
            get { dialogueText = EventMarkers.Instance.ParseAndCall(dialogueText); return dialogueText; }
            set { dialogueText = value; }
        }
    }

    private TextInfo info;

    public TextInfo Info
    {
        get { return info; }
        set { info = value; }
    }

    public string GetTextAndIterate()
    {
        int newIndex = 0;
        string str = Dialogue.GetTextAndIterate(info.fileIndex, info.containerIndex, info.fieldIndex, out newIndex);
        info.DialogueText = str;
        info.fieldIndex = newIndex;
        return str;
    }

    public string GetTextAt(int pIndex)
    {
        string str = Dialogue.GetTextAt(info.fileIndex, info.containerIndex, pIndex);
        info.DialogueText = str;
        info.fieldIndex = pIndex;
        return str;
    }

    public string GetRandomText()
    {
        string str = Dialogue.GetRandomText(info.fileIndex, info.containerIndex);
        info.DialogueText = str;
        return info.DialogueText;
    }
    

    /*public string DialogueText
    {
        get { return dialogueText; }
        set { dialogueText = value; }
    }*/


}
