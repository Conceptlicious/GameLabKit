using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Utility;

public class FireTimer : MonoBehaviour
{
    FireExtinguisherHandler fireExtinguisherHandler => FireExtinguisherHandler.instance;

    [SerializeField] Transform parent;
    [SerializeField] Canvas parentCanvas;
    [SerializeField] Image filledImage;
    float duration;

    List<Vector3> surroundingPositions = new List<Vector3>();

    private void Start()
    {
        duration = Random.Range(1.5f, 2.5f);
        for (int dir = 0; dir < 8; dir++)
        {
            float[] additions = GetAddition(dir);
            float newX = parent.position.x + additions[0];
            float newY = parent.position.z + additions[1];
            if (!(newX >= -4 && newX <= 2) || !(newY >= 4 && newY <= 10)) continue;
            surroundingPositions.Add(new Vector3(newX, 1f, newY));
        }

        Debug.Log("surroundingPositions size: " + surroundingPositions.Count);

        filledImage.fillAmount = 1f;
        StartCoroutine(TimeElapsed(duration));
    }

    public IEnumerator TimeElapsed(float duration)
    {
        float startTime = Time.time;
        float time = duration;
        float value = 1;
        while (Time.time - startTime < duration)
        {
            if (fireExtinguisherHandler.currentState.Equals(States.GAME_OVER))
            {
                parentCanvas.gameObject.SetActive(false);
                yield break;
            }
            time -= Time.deltaTime;
            value = time / duration;
            filledImage.fillAmount = value;
            yield return null;
        }
        if (fireExtinguisherHandler.currentState.Equals(States.GAME_OVER))
        {
            parentCanvas.gameObject.SetActive(false);
            yield break;
        }
        parentCanvas.gameObject.SetActive(false);
        fireExtinguisherHandler.SpawnSurroundingFire(surroundingPositions);
    }

    private float[] GetAddition(int dir)
    {
        switch(dir)
        {
            case NORTH:
                return new float[] { 0, 2 };
            case NORTH_EAST:
                return new float[] { 2, 2 };
            case EAST:
                return new float[] { 2, 0 };
            case SOUTH_EAST:
                return new float[] { 2, -2 };
            case SOUTH:
                return new float[] { 0, -2 };
            case SOUTH_WEST:
                return new float[] { -2, -2 };
            case WEST:
                return new float[] { -2, 0 };
            case NORTH_WEST:
                return new float[] { -2, 2 };
            default:
                return new float[] { 0, 0 };
        }
    }
}
