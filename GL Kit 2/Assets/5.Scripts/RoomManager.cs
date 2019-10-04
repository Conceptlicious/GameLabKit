using GameLab;
using UnityEngine;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This static holds the locations of target objects which the camera will focus when panning around the scene
//as well as the IDs of the current, previous and target locations which it sends to other scripts.
//Usage: Used for camera control and scene transition.
//--------------------------------------------------


public class RoomManager : Singleton<RoomManager>
{
	[Tooltip("The available number of focal points is dictated by the Settings.cs file.")]
	[SerializeField] private Transform[] roomFocalPoints;
	[SerializeField] private Transform focalParent;

	[SerializeField] private int whiteRoomID;
	public int WhiteRoomID => whiteRoomID;

	[SerializeField] private bool alwaysReturnToWhiteRoom;
	[SerializeField] private bool alignFocals;

	private GameObject blankFocal = null;

	//[OLD ORIGIN, ORIGIN, TARGET]
	private Vector3Int currentRoom = new Vector3Int(-1, 0, 0);

	protected override void Awake()
	{
		base.Awake();

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

	/// <summary>
	/// Ensure all rooms share the same Z position.
	/// </summary>
	private void AlignFocalPoints()
	{
		for (int i = 0; i < roomFocalPoints.Length; i++)
		{
			Vector3 position = roomFocalPoints[i].localPosition;
			position.z = Settings.VAL_CAMERA_FOCAL_Z_ALIGNMENT;
			roomFocalPoints[i].localPosition = position;
		}
	}

	/// <summary>
	/// Returns a Vector3 describing the (OLD ORIGIN, ORIGIN, TARGET) room IDs.
	/// </summary>
	/// <returns>A Vector3Int that Josh thought would be a great ID system, god knows why</returns>
	public Vector3Int GetCurrentRoomID()
	{
		return currentRoom;
	}

	/// <summary>
	/// Focuses the camera onto a room sans transition
	/// </summary>
	/// <param name="pID"></param>
	public void SnapFocusRoom(int pID)
	{
		pID = pID % roomFocalPoints.Length;
		CameraSnapEvent newInfo = new CameraSnapEvent(roomFocalPoints[pID], true);
		EventManager.Instance.RaiseEvent(newInfo);
	}

	/// <summary>
	/// Registers all event listeners this class needs to care about.
	/// </summary>
	private void registerAllListeners()
	{
		//EventSystem.RegisterListener(EventType.UI_NEXT_ROOM, OnNextRoomCommand);
		EventManager.Instance.AddListener<NextRoomEvent>(OnNextRoomCommand, 10);
	}


	private void OnNextRoomCommand(NextRoomEvent pInfo)
	{
		currentRoom.x = currentRoom.y;
		currentRoom.y = currentRoom.z;

		if (alwaysReturnToWhiteRoom)
		{
			//If our old target ISN'T the white room, make it the white room. Else make it the id before last room.
			currentRoom.z = currentRoom.z != whiteRoomID ? whiteRoomID : currentRoom.x + 1;
		}
		else
		{
			currentRoom.z++;
		}

		Mathf.Clamp(currentRoom.x, 0, Settings.SYS_VAL_MAX_NUMBER_ROOM_FOCALS);

		LevelProgressEvent newLevelInfo = new LevelProgressEvent(currentRoom.z);
		EventManager.Instance.RaiseEvent(newLevelInfo);

		CameraTargetSelectEvent newInfo = new CameraTargetSelectEvent(roomFocalPoints[currentRoom.y], roomFocalPoints[currentRoom.z], Settings.VAL_CAMERA_ZOOM_DISTANCE, true, true);
		EventManager.Instance.RaiseEvent(newInfo);

	}

	/// <summary>
	/// If a designer has left blank spaces where target objects should be, fill them with empty GOs for safety.
	/// </summary>
	private void FillFocalsWithBlanks()
	{
		for (int i = 0; i < roomFocalPoints.Length; i++)
		{

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
