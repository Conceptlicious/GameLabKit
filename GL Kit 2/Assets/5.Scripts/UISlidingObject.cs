using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlidingObject : MonoBehaviour
{    
    [SerializeField] private GameObject mainObject;
    [SerializeField] private Transform hiddenPosition;
    [SerializeField] private Transform shownPosition;
    [SerializeField] private Vector2 inOutPercentages;

    public GameObject MainObject => mainObject;
    public Transform HiddenPosition => hiddenPosition;
    public Transform ShownPosition => shownPosition;
    public Vector2 InOutPercentages => inOutPercentages;

    void OnValidate()
    {
            
            float x = inOutPercentages.x;
            float y = inOutPercentages.y;
            
            x = Mathf.Clamp01(x);
            y = Mathf.Clamp01(y);

            //BUG: When typing "0.x" the leading zero forces y to become x and therefore wipe x's data.
            float tempX = x;
            float tempY = y;
            
            y = Mathf.Max(tempX, tempY);
            x = Mathf.Min(tempX, tempY);

            inOutPercentages = new Vector2(x, y);
       
    }
}
