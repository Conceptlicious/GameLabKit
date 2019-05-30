using GameLab;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Tooltip("The available number of focal points is dictated by the Settings.cs file.")]
    [SerializeField] private Transform[] roomFocalPoints;
    [SerializeField] private Transform focalParent;

    [SerializeField] private int whiteRoomID;

    [SerializeField] private bool alwaysReturnToWhiteRoom;
    [SerializeField] private bool alignFocals;

    private GameObject blankFocal = null;

    //[OLD ORIGIN, ORIGIN, TARGET]
    private Vector3Int currentRoom = new Vector3Int(0, 0, 0);

    void Awake()
    {
        //GameData.SetLanguage(GameData.Language.ENGLISH);
        //GameData.Initialised = true;
    }
     
    void Start()
    {
        whiteRoomID = Mathf.Clamp(whiteRoomID, 0, roomFocalPoints.Length);
        registerAllListeners();
        FillFocalsWithBlanks();
        if (alignFocals)
        {
            AlignFocalPoints();
        }
        SnapFocusRoom(0);
    }

    private void AlignFocalPoints()
    {
        for (int i = 0; i < roomFocalPoints.Length; i++)
        {
            Vector3 position = roomFocalPoints[i].localPosition;
            position.z = Settings.VAL_CAMERA_FOCAL_Z_ALIGNMENT;
            roomFocalPoints[i].localPosition = position;
        }
    }

	private void Update()
	{
		if(Input.touchSupported && Input.GetMouseButtonDown(0))
		{
			registerAllListeners();
		}
	}

	private void SnapFocusRoom(int pID)
    {
        pID = pID % roomFocalPoints.Length;
        CameraSnapEvent newInfo = new CameraSnapEvent(roomFocalPoints[pID]);
        EventManager.Instance.RaiseEvent(newInfo);
    }
    
    /// <summary>
    /// Registers all event listeners this class needs to care about.
    /// </summary>
    private void registerAllListeners()
    {
        //EventSystem.RegisterListener(EventType.UI_NEXT_ROOM, OnNextRoomCommand);
        EventManager.Instance.AddListener<NextRoomEvent>(OnNextRoomCommand);
    }

    

    private void OnNextRoomCommand(NextRoomEvent pInfo)
    {
        
        Debug.Log("heard");
        //0, 0, 0
        //0, 0, 3
        
        //0, 0, 3
        //0, 3, 3
        currentRoom.x = currentRoom.y;
        currentRoom.y = currentRoom.z;
        
        if (alwaysReturnToWhiteRoom)
        {
           
            //If our old target ISN'T the whiteroom, make it the white room. Else make it the id before last room.
            currentRoom.z = currentRoom.z != whiteRoomID ? whiteRoomID : currentRoom.x + 1;
        }
        else
        {
            currentRoom.z++;
        }
        
       
        Mathf.Clamp(currentRoom.x, 0, Settings.SYS_VAL_MAX_NUMBER_ROOM_FOCALS);   
        //Focal A is current. Focal B is next. Current[New, Old]
        //Debug.Log("focus - x: " + currentRoom.x + " | y: " + currentRoom.y);
        
        LevelProgressEvent newLevelInfo = new LevelProgressEvent(currentRoom.z);
        EventManager.Instance.RaiseEvent(newLevelInfo);
        
        CameraTargetSelectEvent newInfo = new CameraTargetSelectEvent(roomFocalPoints[currentRoom.y], roomFocalPoints[currentRoom.z], Settings.VAL_CAMERA_ZOOM_DISTANCE, true, true);
        EventManager.Instance.RaiseEvent(newInfo);
        
    }

    private void FillFocalsWithBlanks()
    {
        for (int i = 0; i < roomFocalPoints.Length; i++)
        {        
            //Debug.Log("Room focal " + i + ": " + roomFocalPoints[i].position);
            if (roomFocalPoints[i] == null)
            {
               
                Debug.Log("Filling " + i + " with a blank GO.");
                roomFocalPoints[i] = CreateBlankFocal().transform;
            }
               
        }
    }

    private GameObject CreateBlankFocal()
    {
        
        if (blankFocal != null)
        {
            return blankFocal;
        }
        else
        {
            string path =
                Settings.PATH_PREFABS + Settings.OBJ_NAME_BLANK_GAMEOBJECT;
            Debug.Log(path);
            blankFocal = GameObject.Instantiate(Resources.Load<GameObject>(path), focalParent);
            if (blankFocal == null)
            {
                Debug.Log("Cannot instantiate " + Settings.OBJ_NAME_BLANK_GAMEOBJECT);
            }
        }

        return blankFocal;
    }
    
    void OnValidate()
    {
        int oldLength = roomFocalPoints.Length;
        if (oldLength != Settings.SYS_VAL_MAX_NUMBER_ROOM_FOCALS)
        {
            
            Transform[] temp = roomFocalPoints;
            roomFocalPoints = new Transform[Settings.SYS_VAL_MAX_NUMBER_ROOM_FOCALS];
            for (int i = 0; i < oldLength; i++)
            {
                roomFocalPoints[i] = temp[i];
            }

           FillFocalsWithBlanks();
            
        }
    }
}
