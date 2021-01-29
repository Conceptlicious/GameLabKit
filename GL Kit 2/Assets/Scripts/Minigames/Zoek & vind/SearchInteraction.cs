using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchInteraction : MonoBehaviour
{
    private SearchHandler searchHandler => SearchHandler.instance;

    [SerializeField] SearchItem searchItem;

    public SearchItem SearchItem
    {
        get => searchItem;
        private set => searchItem = value;
    }

    /// <summary>
    /// Handles the clicking of an object
    /// </summary>
    public void Click()
    {
        searchHandler.ObjectFound(searchItem);
    }
}
