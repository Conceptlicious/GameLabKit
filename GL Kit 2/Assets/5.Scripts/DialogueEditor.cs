using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Helper script for the custom Unity Editor, allowing each object to have specific dialogue assigned to it.
/// </summary>
[CustomEditor(typeof(DialogueObject))]
public class DialogueEditor : Editor {

    string[] areaChoices;
    int areaChoiceIndex = 0;
    private int oldAreaChoiceIndex = 0;
    
    string[] arrayChoices;
    int arrayChoiceIndex = 0;
        
    string[] keywordChoices;
    int keywordChoiceIndex = 0;
    
    public override void OnInspectorGUI()
    {
       
        

        if (DrawDefaultInspector())
        {
            //if is auto
            //do thing
            
        }
        PopulateOptions();
       
    }

    public void OnValidate()
    {
        PopulateOptions();
    }

    private void PopulateOptions()
    {
        DialogueObject dialogueObject = (DialogueObject)target;
        
        areaChoices = Dialogue.GetLevelFileListings();
        oldAreaChoiceIndex = areaChoiceIndex;
        areaChoiceIndex = EditorGUILayout.Popup(areaChoiceIndex, areaChoices);
        if (oldAreaChoiceIndex != areaChoiceIndex)
        {
            Dialogue.LoadAreaDialogue(areaChoices[areaChoiceIndex]);
        }
        
        arrayChoices = Dialogue.GetArrayListings();
        arrayChoiceIndex = EditorGUILayout.Popup(arrayChoiceIndex, arrayChoices);
        
        keywordChoices = Dialogue.GetKeywordListings(arrayChoices[arrayChoiceIndex]);
        keywordChoiceIndex = EditorGUILayout.Popup(keywordChoiceIndex, keywordChoices);
        
        //dialogueObject.SetKeys(arrayChoices[arrayChoiceIndex], keywordChoiceIndex, );

        
        if (GUILayout.Button("Populate"))
        {
            //dialogue.GenerateMap();
        }
        
        EditorUtility.SetDirty(target);
    }
}
