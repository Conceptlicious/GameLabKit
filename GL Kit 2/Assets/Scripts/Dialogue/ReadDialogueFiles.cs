using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// TODO: Convert to Gamelab Oost singleton system
/// TODO: Convert to GameLab Oost Bettermonobehavour system
/// </summary>
public class ReadDialogueFiles : MonoBehaviour
{
    public static ReadDialogueFiles instance;
    
    private Dictionary<string, List<Dialogue>> dialogues = new Dictionary<string, List<Dialogue>>();

    void Awake()
    {
        instance = this;
        
        //Loads all the text files from the dialogue folder
        TextAsset[] dialogueFiles = Resources.LoadAll<TextAsset>("Dialogue");

        foreach (TextAsset textAsset in dialogueFiles)
        {
            List<Dialogue> dialogue = new List<Dialogue>();

            //Creates a list of lines from the text files in the dialogue folder
            List<string> linesInFile = ExtractLines(textAsset.text);
            //Extract all the dialogues from the file
            List<string> extractedDialogue = ExtractFromBody(GetOutputFromList(linesInFile), "<dialogue>", "</dialogue>");

            foreach (string text in extractedDialogue)
            {
                //Extract all the lines from from the dialogue and remove all the white space
                List<string> lines = ExtractLines(text);
                lines.RemoveAll(string.IsNullOrWhiteSpace);

                //Grab the name of the dialogue and remove it from the lines
                string dialogueName = lines[0];
                lines.Remove(dialogueName);
                dialogue.Add(new Dialogue(dialogueName.Remove(dialogueName.Length - 1), lines));
            }

            dialogues.Add(textAsset.name, dialogue);
        }
    }

    /// <summary>
    /// Handles grabbing a dialogue by file name and name of the dialogue
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Dialogue GetDialogue(string fileName, string name)
    {
        //Debug.Log($"fileName: {fileName}, fileName length: {fileName.Length}, name: {name}, name length: {name.Length}");
        Dialogue dialogue = GetDialogue(dialogues[fileName], name);
        if (dialogue == null)
        {
            Debug.LogError("Dialogue is null?");
            return null;
        }
        dialogue.Reset();
        return dialogue;
    }

    /// <summary>
    /// Handles grabbing a dialogue based on a name fron a list of dialogues
    /// </summary>
    /// <param name="dialogues">The list of dialogues</param>
    /// <param name="name">The name of the dialogue</param>
    /// <returns></returns>
    private Dialogue GetDialogue(List<Dialogue> dialogues, string name)
    {
        return dialogues.FirstOrDefault(dialogue => dialogue.dialogueName.Equals(name));
    }

    /// <summary>
    /// Handles turning a large piece of text into separate lines
    /// </summary>
    /// <param name="data">The text that has be converted into seperate lines</param>
    /// <returns>A list of all the lines within the data parameter</returns>
    private List<string> ExtractLines(string data)
    {
        return new List<string>(data.Split('\n'));
    }

    /// <summary>
    /// Handles turning a list of lines back into text
    /// </summary>
    /// <param name="lines">The list of lines</param>
    /// <returns>A text that has been converted from a list of lines thats seperated with new lines</returns>
    private string GetOutputFromList(List<string> lines)
    {
        return lines.Aggregate(String.Empty, (current, line) => current + $"{line}\n");
    }

    /// <summary>
    /// Extract a list of text between 2 points
    /// </summary>
    /// <param name="body">The text you want to extract the text from</param>
    /// <param name="start">Start point</param>
    /// <param name="end">End point</param>
    /// <returns></returns>
    private List<string> ExtractFromBody(string body, string start, string end)
    {
        List<string> stringList = new List<string>();
        bool flag = false;
        while (!flag)
        {
            int startIndex = body.IndexOf(start);
            if (startIndex != -1)
            {
                int num = startIndex + body.Substring(startIndex).IndexOf(end);
                stringList.Add(body.Substring(startIndex + start.Length, num - startIndex - start.Length));
                body = body.Substring(num + end.Length);
            }
            else
                flag = true;
        }
        return stringList;
    }
}
