using System.Collections.Generic;

public class Dialogue
{
    public string dialogueName;
    public List<string> lines;
    public DialogueType dialogueType = DialogueType.NONE;
    public string[] actions;
    
    public Dialogue(string dialogueName, List<string> lines)
    {
        this.dialogueName = dialogueName;
        this.lines = lines;
    }

    public void Reset()
    {
        dialogueType = DialogueType.NONE;
        actions = null;
    }
}