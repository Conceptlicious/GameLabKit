using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class DialogueFile 
{
    private string name = Settings.STR_DEFAULT_DIALOGUE;
    private DialogueContainer[] containers = null;
    public DialogueFile()
    {
        
    }

    public void CreateContainers(int pSize)
    {   
        containers = new DialogueContainer[pSize];
        for (int i = 0; i < pSize; i++)
        {
            containers[i] = new DialogueContainer();
        }
        
    }

    public void SetContainerInfo(int pIndex, string pName, string pKey, string pValue)
    {
        if (containers != null)
        {
            containers[pIndex].Name = pName;
            containers[pIndex].SetInfo(pKey, pValue);
        }
    }

    public DialogueContainer[] GetContainers()
    {
        Debug.Assert(containers != null, Settings.ERR_ASSERT_DIAG_CONTAINERS_NULL);
        return containers;
    }
    
    public DialogueContainer GetContainer(int pIndex)
    {
        Debug.Assert(containers != null, Settings.ERR_ASSERT_DIAG_CONTAINERS_NULL);
        Debug.Assert(pIndex >= 0 && pIndex < containers.Length, Settings.ERR_ASSERT_DIAG_CONTAINERS_INVALID_INDEX);
        return containers[pIndex];
    }
    

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
}
