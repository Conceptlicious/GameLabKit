using GameLab;

public class DialogueKnotCompletedEvent : GameLabEvent
{
	public RoomType CompletedRoomID { get; private set; }
	public string Knot { get; private set; }

	public DialogueKnotCompletedEvent(RoomType completedRoomID, string completedKnot)
	{
		//UnityEngine.Debug.Log($"Completed room ID: {completedRoomID}, completed knot: {completedKnot}");
		CompletedRoomID = completedRoomID;
		Knot = completedKnot;
	}
}