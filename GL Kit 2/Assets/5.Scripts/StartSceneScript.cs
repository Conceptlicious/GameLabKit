using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameLab;

public class StartSceneScript : BetterMonoBehaviour
{
	public void LoadScene()
	{
		SceneManager.LoadScene(1);
	}
}