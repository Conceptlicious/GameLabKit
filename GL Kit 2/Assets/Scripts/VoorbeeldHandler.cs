using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoorbeeldHandler : MonoBehaviour
{
    [SerializeField] GameObject[] exampleObjects;
    [SerializeField] bool pageOpened = false;

    public void Close()
    {
        foreach (GameObject o in exampleObjects)
            o.SetActive(false);

        pageOpened = false;
    }

    public void Open(int index)
    {
        if (pageOpened) return;

        exampleObjects[index].SetActive(true);
        pageOpened = !pageOpened;
    }
}
