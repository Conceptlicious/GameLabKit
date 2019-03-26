using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObject : MonoBehaviour
{
    //Save the GameObject of the Object found.
    [SerializeField]
    private GameObject objectFound;

    private int timerForTouch = 5;
    private int maxTimerForTouch = 5;

    void Update()
    {
        timerForTouch--;
    }

    // When clicking on Object
    void OnMouseDown()
    {
        onClickPuzzleObject();
    }

    private void onClickPuzzleObject()
    {
        //If the game object has been Found and its not ObjectFound then set ObjectFound as Active
        if (!this.gameObject.name.Contains("Found"))
        {
            //Show Object Found
            objectFound.SetActive(true);
            //Hide current GameObject
            this.gameObject.SetActive(false);
        }
        else
        {
            //If you are pressing ObjectFound then show Name/Description of Object else hide it.
            if (objectFound.activeSelf && timerForTouch <= 0)
            {
                objectFound.SetActive(false);
                timerForTouch = maxTimerForTouch;
            }
            else if(!objectFound.activeSelf && timerForTouch <= 0)
            {
                objectFound.SetActive(true);
                timerForTouch = maxTimerForTouch;
            }
        }
    }
}
