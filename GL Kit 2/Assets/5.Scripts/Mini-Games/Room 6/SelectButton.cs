using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class SelectButton : BetterMonoBehaviour
{
    public static Sprite ArtSprite { get; private set; }
    private Image buttonImage;
    private Button selectButton;

    private void Start()
    {
        selectButton = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        selectButton.onClick.AddListener(() => SelectSprite(buttonImage.sprite));
    }

    private void SelectSprite(Sprite selectedSprite)
    {
        if(!ActivePatternManager.Instance.isWon)
        {
            return;
        }

        ArtSprite = selectedSprite;

        EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.ArtStyle));
        EventManager.Instance.RaiseEvent(new NextRoomEvent());
    }
}
