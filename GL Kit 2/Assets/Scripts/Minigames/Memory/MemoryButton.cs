using UnityEngine;
using UnityEngine.UI;

public class MemoryButton : MonoBehaviour
{
    private MemoryHandler memoryHander => MemoryHandler.instance;

    public delegate void OnButtonClicked(MemoryButton memoryButton);
    public OnButtonClicked onButtonClicked;

    [Header("Get the sprite that is linked to this object")]
    [SerializeField] Sprite realSprite;

    public Sprite RealSprite
    {
        get => realSprite;
        private set => realSprite = value;
    }

    [Header("Check if it has been flipped or not")]
    [SerializeField] bool beenFlipped;
    /// <summary>
    /// The time used for the temp flip
    /// </summary>
    float currentTime;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && memoryHander.hasStarted)
        {
            GameObject clickedObject = Utility.GetClickedGameObject();
            if(gameObject.name.Equals(clickedObject?.name))
                Flip(true);
        }
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if(currentTime < 0)
            {
                gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                currentTime = 0;
            }
        }
    }

    /// <summary>
    /// Handles setting the real sprite of this button
    /// </summary>
    /// <param name="sprite"></param>
    public void SetRealSprite(Sprite sprite)
    {
        RealSprite = sprite;
    }

    /// <summary>
    /// Handles flipping the image untill 2 are flipped
    /// </summary>
    public void Flip(bool clicked = true)
    {
        if ((beenFlipped && clicked) || currentTime > 0 || (memoryHander.LevelTransition && clicked)) return;

        gameObject.transform.localEulerAngles = new Vector3(0, clicked ? -90 : 0, 0);

        beenFlipped = !beenFlipped;
        if(clicked) ButtonClicked(this);
    }

    /// <summary>
    /// Handles temporary flipping the image for 3 seconds
    /// </summary>
    public void TempFlip()
    {
        beenFlipped = false;
        gameObject.transform.localEulerAngles = new Vector3(0, -90, 0);
        currentTime = 3f;
    }

    void ButtonClicked(MemoryButton memoryButton)
    {
        onButtonClicked?.Invoke(memoryButton);
    }

    public void Reset()
    {
        gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
