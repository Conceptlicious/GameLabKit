using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyRotation : MonoBehaviour
{
    [SerializeField] bool target;
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0, target ? 0.25f : 0, target ? 0 : 0.25f));
    }
}
