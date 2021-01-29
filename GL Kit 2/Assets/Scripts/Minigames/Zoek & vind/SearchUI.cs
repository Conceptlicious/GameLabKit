using UnityEngine;
using UnityEngine.UI;

public class SearchUI : MonoBehaviour
{
    private SearchHandler searchHandler => SearchHandler.instance;

    public static SearchUI instance;

    [Header("All objects that need to be found")]
    [SerializeField] SearchItem[] searchItems;

    [Header("UI Elements")]
    [SerializeField] GameObject uiPrefab;
    [SerializeField] GameObject[] uiElements;

    private void Start()
    {
        instance = this;
        searchHandler.onObjectFound += OnObjectFound;
        uiElements = new GameObject[searchItems.Length];
        Reset();
    }

    /// <summary>
    /// Handles the subscription to the OnObjectFound event within search handler
    /// </summary>
    /// <param name="searchItem"></param>
    private void OnObjectFound(SearchItem searchItem)
    {
        UpdateUI(searchItem.name);
    }

    /// <summary>
    /// Handles updating the opacity of the UI sprite
    /// </summary>
    /// <param name="name"></param>
    void UpdateUI(string name)
    {
        foreach(GameObject gObject in uiElements)
        {
            if(gObject.name.Equals(name))
            {
                Image image = gObject.GetComponent<Image>();
                Color color = image.color;
                image.color = new Color(color.r, color.g, color.b, 1);
                break;
            }
        }
    }

    public void Reset()
    {
        for (int index = 0; index < searchItems.Length; index++)
        {
            SearchItem searchItem = searchItems[index];
            GameObject ui = Instantiate(uiPrefab, Vector3.zero, Quaternion.identity);
            ui.transform.SetParent(transform);
            ui.name = searchItem.name;

            RectTransform rect = ui.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(searchItem.sprite.rect.width, searchItem.sprite.rect.height);

            Image image = ui.GetComponent<Image>();
            image.sprite = searchItem.sprite;

            uiElements[index] = ui;
        }
    }
}
