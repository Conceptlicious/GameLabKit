using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerHandler : MonoBehaviour
{

    private VideoPlayer videoPlayer;
    [SerializeField] GameObject mainMenu;

    private void Start()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }

    private void EndReached(VideoPlayer source)
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
