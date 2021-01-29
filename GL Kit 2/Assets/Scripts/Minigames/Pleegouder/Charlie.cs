using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charlie : MonoBehaviour
{
    private OptionHandler optionHandler => OptionHandler.instance;
    private PleegouderHandler pleegouderHandler => PleegouderHandler.instance;

    public static Charlie instance;

    private Vector3 startLocation;

    [SerializeField] Transform[] locations;

    [SerializeField] State currentState;

    [SerializeField] MeshRenderer charlieMesh;

    [SerializeField] GameObject[] charlieObjects;

    public State CurrentState
    {
        get => currentState;
        set => currentState = value;
    }

    public void Start()
    {
        startLocation = transform.position;
        instance = this;
        CurrentState = State.IDLE;
    }

    public void Update()
    {
        if(CurrentState.Equals(State.WALKING))
        {
            transform.position = Vector3.MoveTowards(transform.position, locations[0].position, 2 * Time.deltaTime);
            if (Vector3.Distance(transform.position, locations[0].position) < 0.001f)
            {
                charlieMesh.enabled = false;
                charlieObjects[0].SetActive(true);
                optionHandler.SetOptions(pleegouderHandler.GetCurrentOption());
                transform.position = locations[1].position;
                CurrentState = State.SITTING;
            }
        }
    }

    public void SetCharlieOnChair()
    {
        charlieObjects[0].SetActive(false);
        charlieObjects[1].SetActive(true);
        transform.position = locations[2].position;
    }

    public void Reset()
    {
        charlieMesh.enabled = true;
        charlieObjects[0].SetActive(false);
        charlieObjects[1].SetActive(false);
        transform.position = startLocation;
        CurrentState = State.IDLE;
    }

    public enum State
    {
        IDLE,
        WALKING,
        SITTING
    }
}
