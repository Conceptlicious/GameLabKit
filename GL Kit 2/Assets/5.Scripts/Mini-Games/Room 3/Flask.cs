using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[RequireComponent(typeof(Image))]
public class Flask : BetterMonoBehaviour
{
    public static Sprite FlaskSprite { get; private set; }

    public void OnFlaskSelected()
    {
        //Set the trophy to this
        FlaskSprite = GetComponent<Image>().sprite;

        EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Genre));
        EventManager.Instance.RaiseEvent(new NextRoomEvent());
    }
}
