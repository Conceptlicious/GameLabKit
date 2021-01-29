using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireExtinguisherHandler : Minigame
{
    public static FireExtinguisherHandler instance;

    [Header("Progress of game")]
    public States currentState = States.IDLE;
    [Header("Player data")]
    [SerializeField] int playerLives = 3;
    [SerializeField] int score;
    [SerializeField] int blueFires;
    [SerializeField] int orangeFires;
    [Header("Fire prefabs")]
    [SerializeField] GameObject[] firePrefabs;
    [Header("Spawn variables")]
    private List<Vector3> possibleSpawnPoints = new List<Vector3>();
    private List<Vector3> occupiedSpawnPoints = new List<Vector3>();
    [SerializeField] float spawnTimer = 2.5f;
    [Header("Electric fires")]
    private List<GameObject> electrcFires = new List<GameObject>();
    [Header("Game over")]
    [SerializeField] GameObject scoreBoard;
    [SerializeField] TMP_Text scoreOutcome;
    [Header("Scoreboard")]
    [SerializeField] TMP_Text scoreBoardText;

    private void Start()
    {
        instance = this;
        //Handles setting up all the spawn positions of a fire
        float startX = 2, startY = 1f, startZ = 10; //.8f
        for (int width = 0; width < 4; width++)
        {
            for (int height = 0; height < 4; height++)
            {
                Vector3 spawnLocation = new Vector3(startX, startY, startZ);
                possibleSpawnPoints.Add(spawnLocation);
                startZ -= 2;
            }
            startX -= 2;
            startZ = 10;
        }
        base.Start();
    }

    private void Update()
    {
        //Dont handle the update if the game is over
        if (currentState.Equals(States.GAME_OVER) || !hasStarted) return;

        //Handle possible idle updates
        if (currentState.Equals(States.IDLE)) return;

        //Handles clicking the game objects
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clickedObject = GetClickedGameObject();
            if (clickedObject != null && clickedObject.name.StartsWith("Fire"))
                RemoveFire(clickedObject.transform);
        }

        if (currentState.Equals(States.NO_BLUES) || currentState.Equals(States.IN_GAME))
        {
            //Handles the spawning of a random fire
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                int roll = Random.Range(0, possibleSpawnPoints.Count);
                SpawnFire(possibleSpawnPoints[roll]);
                spawnTimer = Random.Range(1f, 1.5f);
            }
        }
    }

    public override void StartMinigame()
    {
        currentState = States.NO_TIMER;
        for (int index = 0; index < 10; index++)
        {
            int random = Random.Range(0, possibleSpawnPoints.Count);
            SpawnFire(possibleSpawnPoints[random]);
        }
    }

    /// <summary>
    /// Handles spawning a fire on a new position
    /// </summary>
    /// <param name="position"></param>
    void SpawnFire(Vector3 position)
    {
        if (occupiedSpawnPoints.Contains(position)) return;

        int start = currentState.Equals(States.NO_TIMER) ? 0 : 1;
        int length = currentState.Equals(States.IN_GAME) ? firePrefabs.Length : currentState.Equals(States.NO_BLUES) ? 1 : 0;
        int roll = Random.Range(start, length);
        bool electricFire = roll == 2;

        GameObject fire = Instantiate(firePrefabs[roll], position, Quaternion.identity);
        fire.transform.name = $"Fire[{position.x},{position.y},{position.z}]";
        fire.transform.SetParent(transform);

        //Handles adding the fire to the electric fire list if its a blue fire
        if (electricFire)
        {
            Debug.LogError("added electrc fire");
            electrcFires.Add(fire);
        }

        //Handles filling and emptying the lists of spawns/occupied spawns
        possibleSpawnPoints.Remove(position);
        occupiedSpawnPoints.Add(position);
    }

    /// <summary>
    /// Handles removing a fire from the position the player clicked
    /// </summary>
    /// <param name="parent"></param>
    public void RemoveFire(Transform parent)
    {
        //Debug.Log($"Clicked {parent.name}");
        //Debug.Log($"transform.gameObject {parent.gameObject}");

        if (electrcFires.Contains(parent.gameObject))
        {
            Debug.Log("Clicked an electric fire");
            RemoveLife();
            return;
        }

        Destroy(parent.gameObject);
        possibleSpawnPoints.Add(parent.position);
        occupiedSpawnPoints.Remove(parent.position);

        if (currentState.Equals(States.NO_BLUES) || currentState.Equals(States.IN_GAME))
            score += 10;

        scoreBoardText.text = $"Score: {score}";

        orangeFires++;
        if(orangeFires == 10 && currentState.Equals(States.NO_TIMER))
        {
            spawnTimer = 0.25f;
            orangeFires = 0;
            currentState = States.NO_BLUES;
        }
        else if(orangeFires == 8 && currentState.Equals(States.NO_BLUES))
        {
            currentState = States.IN_GAME;
        }
    }

    /// <summary>
    /// Handles spawning a new fire around the fire thats timer ran out
    /// </summary>
    /// <param name="surroundingPositions"></param>
    public void SpawnSurroundingFire(List<Vector3> surroundingPositions)
    {
        do
        {
            Vector3 pos = surroundingPositions[Random.Range(0, surroundingPositions.Count)];
            if (occupiedSpawnPoints.Contains(pos))
            {
                surroundingPositions.Remove(pos);
            }
            else
            {
                SpawnFire(pos);
                break;
            }
        } while (surroundingPositions.Count > 0);
        RemoveLife();
    }

    /// <summary>
    /// Handles removing a life of the player
    /// </summary>
    private void RemoveLife()
    {
        playerLives--;
        if (playerLives <= 0)
        {
            Debug.Log("Ran out of lives...");
            currentState = States.GAME_OVER;

            scoreBoard.SetActive(true);
            scoreOutcome.text = $"Score: {score}\nOranje vuurtjes geblust: {orangeFires}\nBlauwe vuurtjes geblust: {blueFires}";
        }
    }

    /// <summary>
    /// Handles removing all the blue fires from the scene when the player clicks the button
    /// </summary>
    public void TurnOffElectric()
    {
        if (currentState.Equals(States.GAME_OVER) || !hasStarted) return;

        foreach (GameObject fire in electrcFires)
        {
            possibleSpawnPoints.Add(fire.transform.position);
            occupiedSpawnPoints.Remove(fire.transform.position);
            Destroy(fire);
        }

        blueFires += electrcFires.Count;
        electrcFires.Clear();
    }

    public override void FinishMinigame()
    {
        minigameHandler.ReturnToLab();
        dialogueHandler.SetDialogue("Prof. Henk", "Blussen Finished");
        labHandler.roetObject.SetActive(true);
        labHandler.CurrentState = LabHandler.State.TURN_PORTAL_ON;
    }

    public override void OnDialogueFinished()
    {

    }

    public override void ResetMinigame()
    {

    }
}

public enum States
{
    IDLE,
    NO_TIMER,
    NO_BLUES,
    IN_GAME,
    GAME_OVER,
    TUTORIAL
}
