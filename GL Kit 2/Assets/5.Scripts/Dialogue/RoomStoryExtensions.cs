using System.Collections.Generic;
using System.Linq;

public static class RoomStoryExtensions
{
	public static RoomStory GetStoryByID(this List<RoomStory> roomStories, RoomType roomID)
	{
		return roomStories.First(roomStory => roomStory.RoomID == roomID);
	}
}