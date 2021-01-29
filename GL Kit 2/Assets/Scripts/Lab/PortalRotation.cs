using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRotation : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0, 0.1f, 0));
    }
}
